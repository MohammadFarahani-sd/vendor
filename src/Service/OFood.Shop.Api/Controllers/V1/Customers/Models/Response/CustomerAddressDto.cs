using Framework.Core.Spatial;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Api.Controllers.V1.Customers.Models.Response;

[Serializable]
public class CustomerAddressDto
{
    public Guid Id { get; set; }
    public string Address { get; set; } = null!;
    public GeoLocation? Location { get; set; } 
    public static CustomerAddressDto Build(CustomerAddressResponse model)
    {
        return Build<CustomerAddressDto>(model);
    }

    private static T Build<T>(CustomerAddressResponse model) where T : CustomerAddressDto, new()
    {
        return new T
        {
            Id = model.Id,
            Address = model.Address,
            Location = model.Location
        };
    }
}