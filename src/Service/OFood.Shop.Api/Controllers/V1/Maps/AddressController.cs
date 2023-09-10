using Framework.Core.Security.Authorization;
using Framework.Web.Api.Models;
using MapService;
using Microsoft.AspNetCore.Mvc;

namespace OFood.Shop.Api.Controllers.V1.Maps;

[ApiController]
[Route("app-api/map")]
[TypeFilter(typeof(Filters.AuthorizeAttribute), Arguments = new object[] { new[] { Role.Customeer } })]
public class MapController : ControllerBase
{
    private readonly IMapApiService _mapApiService;
    private readonly ILogger<MapController> _logger;

    public MapController(ILogger<MapController> logger, IMapApiService mapApiService)
    {
        _logger = logger;
        _mapApiService = mapApiService;
    }

    [HttpGet("get-address/latitude/{latitude}/longitude/{longitude}")]
    public async Task<ActionResult<string>> Get(double latitude, double longitude)
    {
        var address = await _mapApiService.GetAddressByLocationAsync(latitude, longitude);
        var result = new Response<ParsiMapApiResponse>(address);

        return Ok(result);
    }
}
