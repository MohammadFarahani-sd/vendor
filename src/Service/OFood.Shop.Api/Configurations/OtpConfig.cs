namespace OFood.Shop.Api.Configurations;
public class OtpConfig 
{
    public int OtpSmsSeconds { get; set; } = 120;
    public int ValidityMinutes { get; set; } = 5;
    public int CodeLength { get; set; } = 5;
}
