using Framework.Core.Security.Authorization;

namespace Framework.Core.Security.Token;

public class TokenGenerationSettings
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }

    public string SecurityKey { get; set; } = null!;
}