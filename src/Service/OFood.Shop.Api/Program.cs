using System.Text.Json.Serialization;
using Autofac.Extensions.DependencyInjection;
using CacheHelper;
using Framework.Core.ExceptionHandling;
using Framework.Core.Logging;
using Framework.Logging.Sentry;
using Framework.Web.Api.ExceptionPolicies;
using Framework.Web.Api.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Prometheus;
using RedisConnectionHelper;
using Sentry.AspNetCore;
using Sentry.Extensibility;
using Serializer;
using OFood.Shop.Api.Configurations.EventBus;
using OFood.Shop.Api.Middlewares;
using OFood.Shop.Api.Seed;
using OFood.Shop.Application.Command.Customers;
using OFood.Shop.Common.Constants;
using OFood.Shop.Query.Handlers.Customers;
using OFood.Shop.Query.Handlers.AreaCities;
using Common.BuildingBlocks.IntegrationEventLogEF;
using OFood.Shop.Config;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MapService.MapConfigs;
using OFood.Shop.Infrastructure.Persistence;
using OFood.Shop.Api.Configurations;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Security;
using Framework.Web.Api.Swagger;
using SmsService;
using SmsService.SmsConfigs;

var builder = WebApplication.CreateBuilder(args);

//#if !DEBUG
//builder.WebHost.UseSentry();
//#endif

builder.Services.AddTransient<IExceptionHandler, ExceptionHandler>();
var policies = typeof(DomainExceptionFaultProvider).Assembly.DefinedTypes.Where(t => !t.IsAbstract && t.ImplementedInterfaces.Contains(typeof(IExceptionPolicy)));
foreach (var policy in policies)
{
    builder.Services.AddTransient(typeof(IExceptionPolicy), policy);
}

builder.Services.RegisterInterfaces(builder.Configuration);

builder.Services.AddCacheServices(builder.Configuration.GetSection("Cache"));

builder.Services.AddRedis(builder.Configuration.GetSection("Redis"));

builder.Services.AddSingleton<ISerializer, BinarySerializer>();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new UrlSegmentApiVersionReader());
});
builder.Services.AddVersionedApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});


builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddCors(options => options.AddPolicy("AllowCors", policyBuilder =>
{
    policyBuilder.SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.ConfigureOptions<ConfigureSwaggerUiOptions>();
builder.Services.ConfigureOptions<ConfigureApiBehaviorOptions>();


builder.Services.Configure<HostOptions>(builder.Configuration.GetSection("HostOptions"));
builder.Services.Configure<EventBusSetting>(builder.Configuration.GetSection("EventBusSetting"));
builder.Services.Configure<OtpConfig>(builder.Configuration.GetSection("OtpConfig"));
builder.Services.Configure<TokenExpirationConfig>(builder.Configuration.GetSection("TokenExpirationConfig"));

builder.Services.AddScoped<IUserFactory, UserFactory>();
builder.Services.AddTransient<ISentryEventProcessor, SentryEventProcessor>();
builder.Services.AddTransient<ISentryEventExceptionProcessor, SentryEventExceptionProcessor>();

builder.Services.Configure<EventBusSubscriptionClients>(builder.Configuration.GetSection("EventBusSubscriptionClient"));

builder.Services.AddMediatR(typeof(CreateCustomerCommandHandler).Assembly);

builder.Services.AddCustomHealthCheck(builder.Configuration);

if (builder.Configuration.GetValue<bool>(AppSettingKeys.RunEventBus))
{
    builder.Services.RabbitRegisterServiceCollection(builder.Configuration.GetSection("EventBus")).AddIntegrationServices();
}

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.Configure<MapApiConfig>(builder.Configuration.GetSection("MapApiConfig"));
builder.Services.Configure<SmsConfig>(builder.Configuration.GetSection("SmsConfig"));
builder.Services.AddMediatR(typeof(GetCustomerQueryHandler).Assembly);
builder.Services.AddMediatR(typeof(GetAreaCitiesQueryHandler).Assembly);
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IOtpSmsService, OtpSmsService>();
builder.Services.Configure<JwtBearerTokenSettings>(builder.Configuration.GetSection("JwtBearerTokenSettings"));
var baseAuthenticationProviderKey = builder.Configuration.GetValue<string>("JwtBearerTokenSettings:SecretKey");
var baseAuthentication = builder.Configuration.GetValue<string>("BaseAuthenticationSchema");


builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(baseAuthentication!, options =>
    {
        options.SaveToken = false;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(baseAuthenticationProviderKey!)),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();


//#if !DEBUG
//app.UseSentryTracing();
//#endif


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<InfrastructureDbContext>();
        var userManagementContext = services.GetRequiredService<ApplicationDbContext>();
        var integrationEventLogContext = services.GetRequiredService<IntegrationEventLogContext>();
        var seedLogWriter = services.GetService<ILogWriter<ShopDbContextSeed>>()!;
        var env = services.GetService<IWebHostEnvironment>();

        var seedClass = new ShopDbContextSeed();
        seedClass.SeedMigrationAsync(dbContext, env!, seedLogWriter).Wait();
        seedClass.SeedMigrationAsync(integrationEventLogContext, seedLogWriter).Wait();
        seedClass.SeedMigrationAsync(userManagementContext, env!, seedLogWriter).Wait();


    }
    catch (Exception exception)
    {
        var programLogWriter = services.GetService<ILogWriter<Program>>()!;
        programLogWriter.LogError(exception.Message);
    }
}

app.UseSwagger();

app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("AllowCors");

app.UseMiddleware<LocalizationMiddleware>();

app.UseMiddleware<JwtMiddleware>();

app.UseRouting();

app.UseHttpMetrics();

app.UseAuthorization();

app.UseAuthentication();

app.MapHealthChecks("/health-food-F7CB6D8A-B5F5-4160-B97D-25BE76A39F68");

app.MapControllers();

app.MapMetrics();

app.Run();

namespace OFood.Shop.Api
{
    public class Program
    {
    }
}