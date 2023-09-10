using MapService.MapConfigs;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MapService;

public class MapApiService : IMapApiService
{
    public MapApiConfig _apiConfig;
    private readonly ILogger<MapApiService> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public MapApiService(ILogger<MapApiService> logger, IHttpClientFactory clientFactory, IOptions<MapApiConfig> apiConfig)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _apiConfig = apiConfig.Value;

    }

    public async Task<ParsiMapApiResponse> GetAddressByLocationAsync(double latitude, double longitude)
    {
        return await GetAddressAsync(latitude, longitude);
    }

    private async Task<ParsiMapApiResponse> GetAddressAsync(double latitude, double longitude)
    {
        try
        {
            var client = CreateClient();
            var resultOfAddress = await GetAddress(client, latitude, longitude);
            return resultOfAddress!;
        }
        catch (Exception ex)
        {
            var s = "Exception on getting address";
            _logger.LogError($"API response exception ==> {s} ==> {ex.GetBaseException().Message}");
        }
        return null;
    }


    private async Task<ParsiMapApiResponse> GetAddress(HttpClient client, double latitude, double longitude)
    {
        var query = new Dictionary<string, string>
        {
            ["key"] = _apiConfig.PMIAPITOKEN,
            ["location"] = $"{latitude},{longitude}",
            ["local_address"] = "false",
            ["approx_address"] = "false",
            ["subdivision"] = "false",
            ["plate"] = "false",
            ["request_id"] = "false"
        };

        var response = await client.GetAsync(QueryHelpers.AddQueryString(_apiConfig.BaseAddress, query));

        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        var parsiMapApiResponse = JsonConvert.DeserializeObject<ParsiMapApiResponse>(jsonContent);

        return parsiMapApiResponse!;

    }

    private HttpClient CreateClient()
    {
        var client = _clientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);

        if (client.BaseAddress == null)
            client.BaseAddress = new Uri(_apiConfig.BaseAddress);

        return client;
    }
}
