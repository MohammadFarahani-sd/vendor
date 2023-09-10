using Framework.Core.Query;
using OFood.Shop.Query.Models.AreaCities.Response;

namespace OFood.Shop.Facade.AreaCities
{
    public interface IAreaCityFacade
    {
        /// <summary>
        /// getting area and cities from query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<CollectionItems<AreaCityResponse>> GetAreaCitiesAsync(PaginationFilter filter);

    }
}