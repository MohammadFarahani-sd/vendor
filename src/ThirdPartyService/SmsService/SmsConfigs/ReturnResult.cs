using System.IO.Pipelines;

namespace SmsService.SmsConfigs;

public class ReturnResult
{
    public Result @Return { get; set; }
    public object entries { get; set; }
}
