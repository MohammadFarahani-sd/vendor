using System;
using MediatR;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Query.Models.Customers.Request;

[Serializable]
public class GetCustomerByPhoneNumberQuery : IRequest<CustomerResponse?>
{
    public GetCustomerByPhoneNumberQuery(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public string PhoneNumber { get; set; }
}