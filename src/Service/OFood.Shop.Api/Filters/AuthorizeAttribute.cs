using Framework.Core.Security.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OFood.Shop.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role[] _allowedRoles;
    private readonly IUserContext _userContext;

    public AuthorizeAttribute(IUserContext userContext, params Role[] allowedRoles)
    {
        _userContext = userContext;
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (_userContext == null || _userContext.UserId == Guid.Empty)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        if (_allowedRoles.Any() && !_allowedRoles.Contains(_userContext.Role))
        {
            throw new ForbiddenException("Forbidden");
        }
    }
}