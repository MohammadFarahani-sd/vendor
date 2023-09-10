using Framework.Core.Query;
using MediatR;
using OFood.Shop.Query.Models.AreaCities.Request;
using OFood.Shop.Query.Models.AreaCities.Response;

namespace OFood.Shop.Facade.AreaCities;

public class AreaCityFacade : IAreaCityFacade
{
    private readonly IMediator _mediator;

    public AreaCityFacade(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<CollectionItems<AreaCityResponse>> GetAreaCitiesAsync(PaginationFilter filter)
    {
        var areaCitiesQuery = new GetAreaCitiesQuery(filter.Count, filter.Offset, filter.Keyword);
        return _mediator.Send(areaCitiesQuery);
    }
}