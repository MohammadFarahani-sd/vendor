using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Framework.Core.Security.Authorization;
using Framework.Core.Security.Token;
using Microsoft.IdentityModel.Tokens;

namespace Framework.Security;

public class JwtTokenGenerator : ITokenGenerator
{
    public string GenerateToken(TokenGenerationSettings settings)
    {
        var key = Encoding.ASCII.GetBytes(settings.SecurityKey);
        var claim = new ClaimSet
        {
            {ClaimType.UserId, settings.UserId.ToString()},
            {ClaimType.Username, settings.Username},
            {ClaimType.Role, settings.Role.ToString()}
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateSubject(claim),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var result = tokenHandler.WriteToken(token);

        return result;
    }

    private static ClaimsIdentity GenerateSubject(ClaimSet claimSet)
    {
        var items = new List<Claim>(claimSet.Count);
        items.AddRange(claimSet.Select(item => new Claim(item.Key, item.Value)));
        return new ClaimsIdentity(items);
    }
}