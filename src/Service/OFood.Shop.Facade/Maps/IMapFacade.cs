using MapService;

namespace OFood.Shop.Facade.Maps;

public interface IMapFacade
{
    Task<ParsiMapApiResponse> GetAddressByLocationAsync(double latitude, double longitude);
}
