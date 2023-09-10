using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Framework.Core.TimeProviders;
using Framework.Web.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OFood.Shop.Common.Constants;
using OFood.Shop.TestUtilities.TimeProviders;
using OFood.Shop.TestUtilities.SerializationHelpers;
using OFood.Shop.Api;

namespace OFood.Shop.Integration.Test.BaseTests;

[AutoRollback]
public class BaseIntegrationTest
{
    protected readonly WebApplicationFactory<Program> Application;
    protected readonly IConfiguration Configuration;
    private HttpClient _adminClient;
    private HttpClient _appClient;
    private HttpClient _zoneClient;

    protected BaseIntegrationTest()
    {
        Application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment(EnvironmentNames.Test);

                builder.UseTestServer(testServerOptions => testServerOptions.PreserveExecutionContext = true);

                builder.ConfigureServices(services =>
                {
                    var dateTimeOffsetProvider = services.SingleOrDefault(d => d.ServiceType == typeof(IDateTimeOffsetProvider));
                    if (dateTimeOffsetProvider != null)
                    {
                        services.Remove(dateTimeOffsetProvider);
                    }
                    builder.ConfigureAppConfiguration((c, b) =>
                    {
                        b.AddJsonFile("appsettings.Test.json");
                        b.AddEnvironmentVariables();
                    });
                    var testDateTimeOffsetProvider = new TestDateTimeOffsetProvider();
                    services.AddSingleton<IDateTimeOffsetProvider>(testDateTimeOffsetProvider);
                    services.AddSingleton<IConfigurableDateTimeOffsetProvider>(testDateTimeOffsetProvider);
                });
            });
        Configuration = Application.Server.Services.GetService<IConfiguration>()!;
    }

    protected HttpClient AdminClient
    {
        get
        {
            if (_adminClient?.DefaultRequestHeaders.Authorization != null) return _adminClient;
            _adminClient = Application.CreateClient();
            var token = GetAdminApiTokenAsync().Result;
            _adminClient!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", token);
            return _adminClient;
        }
    }

  
    protected async Task<string> GetAdminApiTokenAsync()
    {
        var email = Configuration.GetSection("admin").Value!;
        var password = Configuration.GetSection("adminpassword").Value!;

        var token = await GetTokenAsync(email, password, "admin/conneect/token");
        return token;
    }


    protected async Task<string> GetTokenAsync(string username, string password, string path, string zoneId = "", string userId = "")
    {
        if (path.Contains("admin"))
        {
            var authentication = new
            {
                email = username,
                Password = password
            };

            var client = Application.CreateClient();
            var authenticationResponse = await client.PostAsJsonAsync(path, authentication);
            authenticationResponse.EnsureSuccessStatusCode();

            var response = await authenticationResponse.Content.ReadFromJsonAsync<Response<AuthenticateResponse>>();
            var token = response.Data.Token;
            return token;
        }
        else
        {
            var authentication = new
            {
                EmployeeId = username,
                Password = password,
                UserId= userId
            };

            var client = Application.CreateClient();
            var authenticationResponse = await client.PostAsJsonAsync(path, authentication);
            authenticationResponse.EnsureSuccessStatusCode();

            var response = await authenticationResponse.Content.ReadFromJsonAsync<Response<AuthenticateResponse>>();
            var token = response.Data.Token;
            return token;
        }
    }
}
