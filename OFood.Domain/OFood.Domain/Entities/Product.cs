using OFood.Domain.Commons;
using OFood.Domain.Contracts;

namespace OFood.Domain.Entities;

public class Product : Entity<long> , IDomainValidation
{
    public bool IsValid()
    {
        throw new NotImplementedException();
    }
}