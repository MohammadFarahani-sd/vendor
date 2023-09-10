using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core.Spatial;
using NetTopologySuite.Geometries;

namespace OFood.Shop.Query.Models.AreaCities.Response;

[Serializable]
public class AreaCityResponse
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; } = null!;
    //public Geometry? Boundary { get; set; }
    public GeoLocation Center { get; set; }
    public string? Slug { get; set; }
    public List<AreaCityResponse> SubAreaCities { get; set; }

    public new static AreaCityResponse Build(AreaCity areaCity)
    {
        var response = Build<AreaCityResponse>(areaCity);
        return response;
    }
    public new static T Build<T>(AreaCity areaCity) where T : AreaCityResponse, new()
    {
        return new T
        {
            Id = areaCity.Id,
            ParentId = areaCity.ParentId,
            Name = areaCity.Name,
            //Boundary = areaCity.Boundary,
            //Center = areaCity.Center == null ? null : new GeoLocation(areaCity.Center!.Y, areaCity.Center.X),
            Slug = areaCity.Slug,
            SubAreaCities = areaCity.SubAreaCities == null ? null : areaCity.SubAreaCities.Select(AreaCityResponse.Build).ToList(),
        };
    }

}