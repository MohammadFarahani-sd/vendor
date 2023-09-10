namespace OFood.Domain.Commons;

public abstract class Entity
{
}

public abstract class Entity<TKey> : Entity where TKey : struct
{
    public virtual TKey Id { get; set; }
}