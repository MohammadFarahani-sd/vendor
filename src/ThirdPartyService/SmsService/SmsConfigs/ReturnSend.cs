namespace SmsService.SmsConfigs;

internal class ReturnSend
{
    public Result @Return { get; set; }
    public List<SendResult> entries { get; set; }
}
