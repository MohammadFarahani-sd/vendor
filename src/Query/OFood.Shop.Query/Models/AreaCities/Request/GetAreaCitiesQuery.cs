using Framework.Core.Query;
using MediatR;
using OFood.Shop.Query.Models.AreaCities.Response;

namespace OFood.Shop.Query.Models.AreaCities.Request;

public class GetAreaCitiesQuery : IRequest<CollectionItems<AreaCityResponse>>
{
    public GetAreaCitiesQuery(int count, int offset, string? keyword)
    {
        Count = count;
        Offset = offset;
        Keyword = keyword;
    }
    public int Count { get; set; }
    public int Offset { get; set; }
    public string? Keyword { get; set; }
}