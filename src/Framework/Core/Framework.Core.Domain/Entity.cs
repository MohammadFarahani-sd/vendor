namespace Framework.Core.Domain;

public abstract class Entity<TId> : ITraceableEntity, ILogicalDeleteEntity
{
    public TId Id { get; set; } = default!;

    public bool IsDeleted { get; set; }

    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}