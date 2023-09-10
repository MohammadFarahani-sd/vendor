using System;
using System.ComponentModel.DataAnnotations.Schema;
using Framework.Core.Spatial;
using OFood.Shop.Query.Models.AreaCities;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Query.Models.Customers;

[Table("CustomerAddresses")]
public class CustomerAddress : Entity
{
    [Column("CustomerId")]
    public Guid CustomerId { get; set; }

    [Column("AreaId")]
    public int AreaId { get; set; }

    [Column("CityId")]
    public int CityId { get; set; }

    [Column("ExtraInfo")]
    public string? ExtraInfo { get; set; }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    [NotMapped]
    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; }


    [ForeignKey("CityId")]
    [NotMapped]
    public virtual AreaCity City { get; set; }

    public CustomerAddressResponse Build()
    {
        return new CustomerAddressResponse()
        {
            Address = GetAddress(),
            Location = GetCustomerLocation(),
            Id = Id,
        };
    }

    private GeoLocation? GetCustomerLocation()
    {
        return new GeoLocation(this.Latitude, this.Longitude) ;
    }

    private string GetAddress()
    {
        return $"{ExtraInfo}";
    }
}
