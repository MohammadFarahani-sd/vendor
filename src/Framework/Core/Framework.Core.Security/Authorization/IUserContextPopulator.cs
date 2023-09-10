namespace Framework.Core.Security.Authorization;

public interface IUserContextPopulator
{
    void Populate(ClaimSet claimSet, string? language);
}