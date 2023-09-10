using System;
using System.Collections.Generic;
using System.Linq;

namespace OFood.Shop.Query.Models.Customers.Response;

[Serializable]
public class CustomerResponse
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public List<CustomerAddressResponse> Addresses { get; set; } = null!;

    public static CustomerResponse Build(Customer customer)
    {
        return Build<CustomerResponse>(customer);
    }

    public static T Build<T>(Customer customer) where T : CustomerResponse, new()
    {
        return new T
        {
            Id = customer.Id,
            PhoneNumber = customer.PhoneNumber,
            Addresses = customer.CustomerAddresses.Select(c => c.Build())
                .OrderByDescending(o=> o.Date)
                .ToList()
        };
    }
}