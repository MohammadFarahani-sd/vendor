namespace SmsService.SmsConfigs;

public class SmsSendRequestModel
{
    public SmsSendRequestModel()
    {
        ParameterArray = new List<SmsParameter>();
    }

    public List<SmsParameter> ParameterArray { get; set; }
    public string Mobile { get; set; }
    public string OtpCode { get; set; }
    public string TemplateId { get; set; }
}
public class SmsParameter
{
    public SmsParameter(string parameter, string parameterValue)
    {
        Parameter = parameter;
        ParameterValue = parameterValue;
    }

    public string Parameter { get; set; }
    public string ParameterValue { get; set; }
}
