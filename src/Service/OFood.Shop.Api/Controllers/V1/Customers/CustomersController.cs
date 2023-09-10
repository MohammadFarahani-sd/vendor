using Framework.Core.Security.Authorization;
using Framework.Web.Api.Controllers;
using Framework.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using OFood.Shop.Api.Controllers.V1.Customers.Models.Response;
using OFood.Shop.Facade.Customers;
using Framework.Core.Domain.Exceptions;
using OFood.Shop.Api.Controllers.V1.Customers.Models.Request;

namespace OFood.Shop.Api.Controllers.V1.Customers;

[ApiController]
[Route("app-api/customers")]
[TypeFilter(typeof(Filters.AuthorizeAttribute), Arguments = new object[] { new[] { Role.Customeer } })]
public class CustomersController : BaseApiController
{
    private readonly ICustomerFacade _facade;
    private readonly IUserContext _userContext;

    public CustomersController(ICustomerFacade facade, IUserContext userContext)
    {
        _facade = facade;
        _userContext = userContext;
    }
    /*
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetGetCustomerByPhoneNumberAsync([FromQuery] string phoneNumber)
    {
        var customer = await _facade.GetCustomerAsync(phoneNumber);
        if (customer == null)
        {
            return Ok(new Response<CustomerDto?>(null));
        }
        var dto = CustomerDto.Build(customer);
        var result = new Response<CustomerDto>(dto);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Response<Guid>>> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
    {
        var customer = await _facade.GetCustomerAsync(request.PhoneNumber);
        if (customer != null)
        {
            throw new DomainException("ThisCustomerIsAlreadyExist");
        }

        var result = await _facade.CreateCustomerAsync(new(
            phoneNumber: request.PhoneNumber));

        var response = new Response<Guid>(result);
        return Created("", response);
    }
    */

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Response<Guid>>> AddAddressToExistCustomerAsync([FromRoute] Guid id, [FromBody] AddCustomerAddressRequest request)
    {
        var result = await _facade.AddAddressToExistCustomerAsync(
            new (id, new(
                request.AreaId, request.CityId,request.ExtraInfo, request.Location)));

        var response = new Response<Guid>(result);
        return Ok(response);
    }
}