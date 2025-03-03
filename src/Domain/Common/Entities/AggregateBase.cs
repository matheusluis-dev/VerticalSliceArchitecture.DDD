using System.Collections.Immutable;

namespace Domain.Common.Entities;

public abstract class AggregateBase : IEntity
{
    private IImmutableList<IDomainEvent> _domainEvents;

    protected AggregateBase(IImmutableList<IDomainEvent>? domainEvents)
    {
        _domainEvents = domainEvents ?? [];
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
