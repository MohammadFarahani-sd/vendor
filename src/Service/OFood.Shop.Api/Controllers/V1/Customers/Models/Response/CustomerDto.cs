using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Api.Controllers.V1.Customers.Models.Response;

[Serializable]
public class CustomerDto
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public List<CustomerAddressDto> Addresses { get; set; } = null!;

    public static CustomerDto Build(CustomerResponse customer)
    {
        return Build<CustomerDto>(customer);
    }

    private static T Build<T>(CustomerResponse model) where T : CustomerDto, new()
    {
        return new T
        {
            Id = model.Id,
            PhoneNumber = model.PhoneNumber,
            Addresses = model.Addresses.Select(CustomerAddressDto.Build).ToList()
        };
    }
}