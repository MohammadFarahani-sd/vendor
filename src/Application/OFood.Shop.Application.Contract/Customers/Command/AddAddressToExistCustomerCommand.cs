using MediatR;
using OFood.Shop.Application.Contract.Customers.Command.CommandRequest;

namespace OFood.Shop.Application.Contract.Customers.Command;

[Serializable]
public class AddAddressToExistCustomerCommand : IRequest<Guid>
{
    public AddAddressToExistCustomerCommand(AddAddressToExistCustomerRequest address)
    {
        AddressInfo=address;
    }

    public AddAddressToExistCustomerRequest AddressInfo { get; set; }
}