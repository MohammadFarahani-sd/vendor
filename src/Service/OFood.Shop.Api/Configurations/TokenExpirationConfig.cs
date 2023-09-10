namespace OFood.Shop.Api.Configurations;
public class TokenExpirationConfig 
{
    public int PasswordClientId { get; set; } //minutes
    public int OtpClientId { get; set; } //minutes - 3 days

    public int OtpExpirationTime { get; set; }

    public int TokenExpriationTime { get; set; }
}
