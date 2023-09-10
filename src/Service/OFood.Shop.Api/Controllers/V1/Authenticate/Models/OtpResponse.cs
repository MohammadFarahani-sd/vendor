namespace OFood.Shop.Api.Controllers.V1.Authenticate.Models;

public class OtpResponse
{
    public OtpResponse(int otpValidation)
    {
        OtpValidationTime = otpValidation;
    }
    public int OtpValidationTime { get; set; }
}
