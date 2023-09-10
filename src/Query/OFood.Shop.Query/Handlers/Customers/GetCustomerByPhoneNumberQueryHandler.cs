using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OFood.Shop.Query.Models.Customers.Mapping;
using OFood.Shop.Query.Models.Customers.Request;
using OFood.Shop.Query.Models.Customers.Response;

namespace OFood.Shop.Query.Handlers.Customers;

public class GetCustomerByPhoneNumberQueryHandler : IRequestHandler<GetCustomerByPhoneNumberQuery, CustomerResponse?>
{
    private readonly QueryDbContext _dbContext;

    public GetCustomerByPhoneNumberQueryHandler(QueryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerResponse?> Handle(GetCustomerByPhoneNumberQuery filter, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .AsNoTracking()
            .Include(c => c.CustomerAddresses)
            .ThenInclude(t => t.City)
            .Where(c => c.PhoneNumber == filter.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken);
      
        return customer == null ? null : Mapper.Map(customer);
    }
}