using Framework.Core.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Sentry;
using Sentry.AspNetCore;

namespace Framework.Logging.Sentry;

public class UserFactory : IUserFactory
{
    private readonly IUserContext _userContext;

    public UserFactory(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public User Create(HttpContext context)
    {
        return new User
        {
            Id = _userContext.UserId.ToString(),
            Username = _userContext.Username,
            Email = _userContext.Role.ToString(),
            IpAddress = context.Connection.RemoteIpAddress?.ToString()
        };
    }
}