namespace OFood.Shop.Api.Controllers.V1.Authenticate.Models;
public class AuthenticationResponse
{
    public string Token { get; set; } = null!;
    public DateTimeOffset Expiration { get; set; }

    public static AuthenticationResponse Build(string token,DateTimeOffset expiration)
    {
        return Build<AuthenticationResponse>(token, expiration);
    }

    private static T Build<T>(string token, DateTimeOffset expiration) where T : AuthenticationResponse, new()
    {
        return new T
        {
            Token = token,
            Expiration = expiration
        };
    }
}
