namespace SmsService.SmsConfigs;

public class SmsTokenResponseModel
{
    public string TokenKey { get; set; }
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
}

