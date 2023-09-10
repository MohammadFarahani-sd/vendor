using Common.BuildingBlocks.IntegrationEventLogEF;
using Framework.Core.Logging;
using Framework.Core.Security.Authorization;
using Framework.Core.Security.Cryptography;
using Framework.Core.Security.Token;
using Framework.Core.TimeProviders;
using Framework.Logging;
using Framework.Security;
using Framework.Security.Cryptography;
using MapService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OFood.Shop.Domain.AggregatesModel.CustomerAggregate;
using OFood.Shop.Facade.AreaCities;
using OFood.Shop.Facade.Customers;
using OFood.Shop.Facade.Maps;
using OFood.Shop.Infrastructure.Domain.Customers;
using OFood.Shop.Infrastructure.Persistence;
using OFood.Shop.Query;
using Redis.StackExchange;
using System.Text;
using UserManagement.Security;

namespace OFood.Shop.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInterfaces(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InfrastructureDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
        });

        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseSqlServer(configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
        });

        services.AddDbContext<QueryDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
        });

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

        // For Identity  
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();      

        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddHttpClient();

        services.AddTransient<IDateTimeOffsetProvider, DefaultDateTimeOffsetProvider>();
        services.AddScoped<IHashProvider, HashProvider>();

        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUserContextPopulator, UserContextPopulator>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddTransient(typeof(ILogWriter<>), typeof(LogWriter<>));

        services.AddDbContext<QueryDbContext>();

        services.AddDbContext<InfrastructureDbContext>();

        services.AddScoped<IAreaCityFacade, AreaCityFacade>();
        services.AddScoped<ICustomerFacade, CustomerFacade>();
        services.AddScoped<IMapFacade, MapFacade>();
        services.AddScoped<IMapApiService, MapApiService>();
        return services;
    }
}
