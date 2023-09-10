using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Framework.Core.Logging;
using Framework.Core.Security.Authorization;
using Microsoft.IdentityModel.Tokens;
using OFood.Shop.Common.Constants;

namespace OFood.Shop.Api.Middlewares;

public class JwtMiddleware
{
    private readonly IConfiguration _configuration;
    private readonly ILogWriter<JwtMiddleware> _logWriter;
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogWriter<JwtMiddleware> logWriter)
    {
        _next = next;
        _configuration = configuration;
        _logWriter = logWriter;
    }

    public async Task Invoke(HttpContext context, IUserContextPopulator userContextPopulator)
    {
        var token = context.Request.Headers[HttpHeaderKeys.Authorization].FirstOrDefault()?.Split(" ").Last();
        var language = context.Request.Headers[HttpHeaderKeys.Language].FirstOrDefault()?[..2];

        if (token != null)
        {
            var authResult = AttachUserToContext(userContextPopulator, token, language);
            if (authResult.Claims != null)
            {
                var role = authResult.Claims.FirstOrDefault(q => q.Type == "Role")!.Value;
                var userId = authResult.Claims.FirstOrDefault(q => q.Type == "UserId")!.Value;
                //TODO check with redis in next step
                //if (role == Role.Customeer.ToString()) await ValidToken(userId, token);
            }
        }

        await _next(context);
    }

    private JwtSecurityToken AttachUserToContext(IUserContextPopulator userContextPopulator, string token,
        string? language)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection(ConfigurationKeys.JwtSecurityKey).Value!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var claimSet = GetClaimSet(jwtToken.Claims);
            userContextPopulator.Populate(claimSet, language);

            return jwtToken;
        }
        catch (Exception ex)
        {
            _logWriter.LogInformation(ex.Message);

            throw new UnauthorizedException("Unauthorized");
        }
    }

    private static ClaimSet GetClaimSet(IEnumerable<Claim> claims)
    {
        var claimSet = new ClaimSet();
        foreach (var claim in claims) claimSet.Add(claim.Type, claim.Value);
        return claimSet;
    }
}