using System.Collections.Immutable;

namespace Domain.Common.Entities;

public abstract class EntityBase : IEntity
{
    private IImmutableList<IDomainEvent> _domainEvents;

    protected EntityBase(IList<IDomainEvent>? domainEvents)
    {
        _domainEvents = domainEvents?.ToImmutableList() ?? [];
    }

    public IImmutableList<IDomainEvent> GetDomainEvents()
    {
        var events = _domainEvents.ToImmutableList();
        _domainEvents = _domainEvents.Clear();

        return events;
    }

    protected internal void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents = _domainEvents.Add(domainEvent);
    }
}
