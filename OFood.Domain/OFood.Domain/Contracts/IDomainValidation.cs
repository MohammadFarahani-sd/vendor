namespace OFood.Domain.Contracts;


/// <summary>
/// This interface will be used for patch operation on grid in the future
/// </summary>
public interface IDomainValidation
{
    bool IsValid();
}