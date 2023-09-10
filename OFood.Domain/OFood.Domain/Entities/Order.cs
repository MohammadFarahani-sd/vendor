using OFood.Domain.Commons;

namespace OFood.Domain.Entities;

public class Order : Entity<long>
{
    public int OrderCount { get; init; }
}