namespace Framework.Core.Domain;

public interface ILogicalDeleteEntity
{
    public bool IsDeleted { get; set; }
}