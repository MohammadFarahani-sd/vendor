using MediatR;

namespace OFood.Shop.Domain.SeedWork;

[Serializable]
public abstract class Entity
{
    private List<INotification> _domainEvents;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    public Guid Id { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;

    public DateTimeOffset? ModifiedAt { get; protected set; }

    public DateTimeOffset CreatedAt { get; protected set; }

    public void AddDomainEvent(INotification domainEvent)
    {
        if (_domainEvents == null)
        {
            _domainEvents = new List<INotification>();
        }

        _domainEvents.Add(domainEvent);
    }
    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    protected static async Task CheckRule(IBusinessRule rule)
    {
        if (await rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule, rule.Properties, rule.ErrorType);
        }
    }

    internal void MarkAsUpdated()
    {
        ModifiedAt = DateTimeOffset.Now;
    }
}
