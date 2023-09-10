using CacheHelper;
using MapService;
using Microsoft.Extensions.Options;

namespace OFood.Shop.Facade.Maps;

public class MapFacade : IMapFacade
{
    private readonly IMapApiService _mapApiService;

    private static readonly TimeSpan Reconstruct = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan Expiration = TimeSpan.FromDays(30);
    private readonly ICacheHelper _cacheHelper;
    private readonly CacheConfiguration _configuration;
    private readonly string _prefix;

    public MapFacade(ICacheHelper cacheHelper, IOptions<CacheConfiguration> configuration)
    {
        _cacheHelper = cacheHelper;
        _configuration = configuration.Value;
        _prefix = $"{_configuration.Prefix}-map-location";
    }

    public Task<ParsiMapApiResponse> GetAddressByLocationAsync(double latitude, double longitude)
    {
        return _cacheHelper.FetchAsync(_prefix, $"{latitude}-{longitude}",
            () => _mapApiService.GetAddressByLocationAsync(latitude, longitude),
            Reconstruct,
            Expiration,
            _configuration.Ttl);
    }
}