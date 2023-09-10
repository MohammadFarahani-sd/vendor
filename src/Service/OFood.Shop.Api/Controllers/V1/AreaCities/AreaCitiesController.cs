using Framework.Core.Query;
using Framework.Core.Security.Authorization;
using Framework.Web.Api.Controllers;
using Framework.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using OFood.Shop.Api.Controllers.V1.AreaCities.Models.Response;
using OFood.Shop.Api.Filters;
using OFood.Shop.Facade.AreaCities;

namespace OFood.Shop.Api.Controllers.V1.AreaCities;

[ApiController]
[Route("app-api/area-cities")]
[TypeFilter(typeof(AuthorizeAttribute), Arguments = new object[] { new[] { Role.Customeer } })]
public class AreaCitiesController : BaseApiController
{
    private readonly IAreaCityFacade _areaCityFacade;
    public AreaCitiesController(IAreaCityFacade areaCityFacade)
    {
        _areaCityFacade = areaCityFacade;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<AreaCityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginationResponse<AreaCityDto>>> GetAreaCitiesAsync([FromQuery] PaginationRequest request)
    {
        var areaCities = await _areaCityFacade.GetAreaCitiesAsync(new PaginationFilter()
        {
            Count = request.Count,
            Offset = request.Offset,
            Keyword = request.Keyword
        });

        var result = new PaginationResponse<AreaCityDto>(areaCities.Items.Select(AreaCityDto.Build), areaCities.TotalCount, "");

        return Ok(result);
    }
}