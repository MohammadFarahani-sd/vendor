using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OFood.Shop.Query.Models;
[Serializable]
public abstract class Entity
{
    private List<INotification> _domainEvents;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly()!;

    public Guid Id { get; protected set; }
    public bool IsDeleted { get; protected set; }=false;
    public DateTimeOffset? ModifiedAt { get; protected set; }

    public DateTimeOffset CreatedAt { get; protected set; }

    protected void AddDomainEvent(INotification domainEvent)
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

}