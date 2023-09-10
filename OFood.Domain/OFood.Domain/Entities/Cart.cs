using OFood.Domain.Commons;

namespace OFood.Domain.Entities;

public class Cart : Entity<long>
{
    public int CartCount { get; set; }

    public long CartIdLong { get; set;}
}