namespace MapService;

public interface IMapApiService
{
    Task<ParsiMapApiResponse> GetAddressByLocationAsync(double latitude, double longitude);
}
