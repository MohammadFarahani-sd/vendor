using Microsoft.EntityFrameworkCore;
using OFood.Shop.Domain.AggregatesModel.CustomerAggregate;
using OFood.Shop.Infrastructure.Persistence;

namespace OFood.Shop.Infrastructure.Domain.Customers;

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(InfrastructureDbContext dbContext) : base(dbContext)
    {
    }

    public Customer Add(Customer entity)
    {
        return DbContext.Customers.Add(entity).Entity;
    }

    public async Task<Customer> GetCustomer(Guid id)
    {
        return await DbContext.Customers.Include(t => t.CustomerAddresses)
              .SingleAsync(c => c.Id == id);
    }
}