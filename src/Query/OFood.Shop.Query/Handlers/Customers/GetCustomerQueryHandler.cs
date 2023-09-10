using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OFood.Shop.Query.Models.Customers.Mapping;
using OFood.Shop.Query.Models.Customers.Request;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Query.Handlers.Customers;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerResponse?>
{
    private readonly QueryDbContext _dbContext;

    public GetCustomerQueryHandler(QueryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerResponse?> Handle(GetCustomerQuery filter, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .Include(c => c.CustomerAddresses)
            .ThenInclude(t => t.City)
            .Where(c => c.Id == filter.CustomerId)
            .Where(q => q.CustomerAddresses.Any(s => s.Id == filter.AddressId))
            .FirstOrDefaultAsync(cancellationToken);
      
        return customer == null ? null : Mapper.Map(customer);
    }
}