namespace OFood.Shop.Domain.SeedWork;
public interface IBusinessRule
{
    string Message { get; }

    string[] Properties { get; }

    string ErrorType { get; }

    Task<bool> IsBroken();
}
