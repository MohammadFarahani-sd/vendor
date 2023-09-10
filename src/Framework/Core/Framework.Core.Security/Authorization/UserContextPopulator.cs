using Framework.Core.Extensions;
using Framework.Core.Languages;

namespace Framework.Core.Security.Authorization;

public class UserContextPopulator : IUserContextPopulator
{
    private readonly IUserContext context;

    public UserContextPopulator(IUserContext userContext)
    {
        context = userContext;
    }

    public void Populate(ClaimSet claimSet, string? language)
    {
        if (context is UserContext userContext)
        {
            var userId = claimSet[ClaimType.UserId];
            userContext.UserId = Guid.Parse(userId);

            userContext.Username = claimSet[ClaimType.Username];
            
            var role = claimSet[ClaimType.Role];
            userContext.Role = Enum.Parse<Role>(role);

            userContext.IsAuthorized = true;

            userContext.ClaimSet = claimSet;

            userContext.Language = language?.GetValueFromName<Language>() ?? Language.English;
        }
    }
}