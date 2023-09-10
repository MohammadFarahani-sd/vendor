using MediatR;
using OFood.Shop.Application.Contract.Customers.Command;
using OFood.Shop.Domain.AggregatesModel.CustomerAggregate;

namespace OFood.Shop.Application.Command.Customers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;
    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer(request.CustomerInfo.Id, request.CustomerInfo.PhoneNumber);

        _repository.Add(entity);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return entity.Id;
    }
}