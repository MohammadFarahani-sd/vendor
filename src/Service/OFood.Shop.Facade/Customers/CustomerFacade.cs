using MediatR;
using OFood.Shop.Application.Contract.Customers.Command;
using OFood.Shop.Application.Contract.Customers.Command.CommandRequest;
using OFood.Shop.Query.Models.Customers.Request;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Facade.Customers;

public class CustomerFacade : ICustomerFacade
{
    private readonly IMediator _mediator;

    public CustomerFacade(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<CustomerResponse?> GetCustomerAsync(string phoneNumber)
    {
        var request = new GetCustomerByPhoneNumberQuery(phoneNumber);
        return _mediator.Send(request);
    }

    public Task<Guid> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var customerInfo = new CreateCustomerCommand(request);
        return _mediator.Send(customerInfo);
    }

    public Task<Guid> AddAddressToExistCustomerAsync(AddAddressToExistCustomerRequest request)
    {
        var customerInfo = new AddAddressToExistCustomerCommand(request);
        return _mediator.Send(customerInfo);
    }
}