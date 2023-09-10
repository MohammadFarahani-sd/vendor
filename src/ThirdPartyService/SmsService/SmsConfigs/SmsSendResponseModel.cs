namespace SmsService.SmsConfigs;

public class SmsSendResponseModel 
{
    public string VerificationCodeId { get; set; }
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
}
