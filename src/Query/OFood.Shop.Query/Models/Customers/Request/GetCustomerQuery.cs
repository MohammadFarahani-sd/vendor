using System;
using MediatR;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Query.Models.Customers.Request;

[Serializable]
public class GetCustomerQuery : IRequest<CustomerResponse?>
{
    public GetCustomerQuery(Guid customerId, Guid addressId )
    {
        CustomerId = customerId;
        AddressId = addressId;
    }

    public Guid  CustomerId { get; set; }
    public Guid AddressId { get; set; }
}