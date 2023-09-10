using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Query.Models.Customers.Mapping;

public static class Mapper
{
    public static CustomerResponse Map(Customer customer)
    {
        return CustomerResponse.Build(customer);
    }
}