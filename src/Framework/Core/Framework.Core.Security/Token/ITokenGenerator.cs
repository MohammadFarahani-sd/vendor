namespace Framework.Core.Security.Token;

public interface ITokenGenerator
{
    string GenerateToken(TokenGenerationSettings settings);
}