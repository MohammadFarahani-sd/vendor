using System;
using System.Linq;
using Framework.Core.Spatial;
using NetTopologySuite.Geometries;

namespace OFood.Shop.Query.Models.Customers.Response;

[Serializable]
public class CustomerAddressResponse
{
    public Guid Id { get; set; }
    public string Address { get; set; } = null!;
    public GeoLocation? Location { get; set; }
    public DateTimeOffset Date { get; set; }
    public static CustomerAddressResponse Build(CustomerAddress customerAddress)
    {
        return Build<CustomerAddressResponse>(customerAddress, "", "");
    }
    public static CustomerAddressResponse Build(CustomerAddress customerAddress, string areaName, string cityName)
    {
        return Build<CustomerAddressResponse>(customerAddress, areaName, cityName);
    }

    public static T Build<T>(CustomerAddress customerAddress, string areaName, string cityName) where T : CustomerAddressResponse, new()
    {
        return new T
        {
            Id = customerAddress.Id,
            Address = customerAddress.ExtraInfo!,
            Date = customerAddress.CreatedAt,
            Location = customerAddress.Latitude == 0 || customerAddress.Longitude == 0 ? null :
                new GeoLocation(customerAddress.Latitude, customerAddress.Longitude)
        };
    }

}