using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Framework.Core.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OFood.Shop.Query.Models.AreaCities.Mapping;
using OFood.Shop.Query.Models.AreaCities.Request;
using OFood.Shop.Query.Models.AreaCities.Response;

namespace OFood.Shop.Query.Handlers.AreaCities;

public class GetAreaCitiesQueryHandler : IRequestHandler<GetAreaCitiesQuery, CollectionItems<AreaCityResponse>>
{
    private readonly QueryDbContext _dbContext;

    public GetAreaCitiesQueryHandler(QueryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CollectionItems<AreaCityResponse>> Handle(GetAreaCitiesQuery filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.AreaCities
            .AsNoTracking()
            .Include(c => c.SubAreaCities);

        var totalCount = await query.LongCountAsync(cancellationToken);

        var areaCities = await query
            .Skip(filter.Offset)
            .Take(filter.Count)
            .Where(c => c.ParentId == null)
            .Select(c => Mapper.Map(c))
            .ToListAsync(cancellationToken);

        return new CollectionItems<AreaCityResponse>(areaCities, totalCount);
    }

}