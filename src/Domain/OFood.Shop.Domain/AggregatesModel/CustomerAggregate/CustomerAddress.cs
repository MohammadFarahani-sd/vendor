using System.ComponentModel.DataAnnotations.Schema;
using Framework.Core.Spatial;
using OFood.Shop.Domain.SeedWork;

namespace OFood.Shop.Domain.AggregatesModel.CustomerAggregate;

[Table("CustomerAddresses")]
public class CustomerAddress : Entity
{
    public Guid CustomerId { get; private set;}
    public int CityId { get; private set; }
    public int AreaId { get; private set; }
    public string? ExtraInfo { get; private set; }
   
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    protected CustomerAddress()
    {
        
    }
    public CustomerAddress(Guid customerId, int areaId, int cityId, string? extraInfo, GeoLocation location)
    {
        this.CustomerId = customerId;
        this.CityId = cityId;
        this.AreaId = areaId;
        this.ExtraInfo = extraInfo;
        this.Latitude = location.Latitude;
        this.Longitude= location.Longitude;
    }

    public GeoLocation GetLocation()
    {
        return new GeoLocation( this.Latitude, this.Longitude);
    }

    public void SetLocation(GeoLocation destinationLocation)
    {
        this.Latitude = destinationLocation.Latitude;
        this.Longitude = destinationLocation.Longitude;
    }

}
