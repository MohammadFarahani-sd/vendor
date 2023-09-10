using MediatR;
using OFood.Shop.Application.Contract.Customers.Command;
using OFood.Shop.Domain.AggregatesModel.CustomerAggregate;
using OFood.Shop.Domain.Exceptions;

namespace OFood.Shop.Application.Command.Customers;

public class AddAddressToExistCustomerCommandHandler : IRequestHandler<AddAddressToExistCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;
    public AddAddressToExistCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> Handle(AddAddressToExistCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetCustomer(request.AddressInfo.CustomerId);
        if (entity == null)
        {
            throw new DomainException("ThisCustomerIsNotAlreadyExist");
        }
        entity.AddCustomerAddress(request.AddressInfo.Address.AreaId, request.AddressInfo.Address.CityId, request.AddressInfo.Address.ExtraInfo, request.AddressInfo.Address.Location);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return entity.CustomerAddresses.LastOrDefault()!.Id;
    }
}