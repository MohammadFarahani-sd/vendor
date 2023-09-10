using Framework.Core.Languages;

namespace Framework.Core.Security.Authorization;

public interface IUserContext
{
    Guid UserId { get; }
    string Username { get; }
    Role Role { get; }
    bool IsAuthorized { get; }
    ClaimSet? ClaimSet { get; }
    Language Language { get; }
}