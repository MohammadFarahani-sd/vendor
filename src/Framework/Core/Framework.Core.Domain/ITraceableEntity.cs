namespace Framework.Core.Domain;

public interface ITraceableEntity
{
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}