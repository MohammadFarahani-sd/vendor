using MediatR;
using OFood.Shop.Application.Contract.Customers.Command.CommandRequest;

namespace OFood.Shop.Application.Contract.Customers.Command;

[Serializable]
public class CreateCustomerCommand : IRequest<Guid>
{
    public CreateCustomerCommand(CreateCustomerRequest customerInfo)
    {
        CustomerInfo = customerInfo;
    }

    public CreateCustomerRequest CustomerInfo { get; set; }
}