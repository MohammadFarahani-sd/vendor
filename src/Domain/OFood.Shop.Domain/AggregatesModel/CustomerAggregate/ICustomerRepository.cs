using OFood.Shop.Domain.SeedWork;

namespace OFood.Shop.Domain.AggregatesModel.CustomerAggregate;

public interface ICustomerRepository : IRepository<Customer>
{
    Customer Add(Customer customer);

    Task<Customer> GetCustomer(Guid id);
}
