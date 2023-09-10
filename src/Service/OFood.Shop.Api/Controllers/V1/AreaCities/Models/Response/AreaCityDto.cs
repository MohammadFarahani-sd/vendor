using OFood.Shop.Query.Models.AreaCities.Response;

namespace OFood.Shop.Api.Controllers.V1.AreaCities.Models.Response;

[Serializable]
public class AreaCityDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string? Slug { get; set; }
    public List<AreaCityDto>? SubAreaCities { get; set; } = new();
    public static AreaCityDto Build(AreaCityResponse areaCity)
    {
        return Build<AreaCityDto>(areaCity);
    }
    private static T Build<T>(AreaCityResponse areaCity) where T : AreaCityDto, new()
    {
        return new T
        {
            Id = areaCity.Id,
            ParentId = areaCity.ParentId,
            Name = areaCity.Name,
            Slug = areaCity.Slug,
            SubAreaCities = areaCity.SubAreaCities == null ? null : areaCity.SubAreaCities.Count == 0 ? null : areaCity.SubAreaCities.Select(Build).ToList()
        };
    }
}