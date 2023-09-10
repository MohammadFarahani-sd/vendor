using OFood.Shop.Query.Models.AreaCities.Response;

namespace OFood.Shop.Query.Models.AreaCities.Mapping;

public static class Mapper
{
    public static AreaCityResponse Map(AreaCity areaCity)
    {
        return AreaCityResponse.Build(areaCity);
    }
}