namespace Framework.Web.Api.Models;

public class ResponseMeta
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public MessageType MessageType { get; set; }
}