using Framework.Core.Languages;

namespace Framework.Core.Security.Authorization;

public class UserContext : IUserContext
{
    public Guid UserId { get; set; }

    public string Username { get; set; }

    public Role Role { get; set; }

    public bool IsAuthorized { get; set; }

    public ClaimSet? ClaimSet { get; set; }

    public Language Language { get; set; }
}