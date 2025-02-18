namespace Domain.Common.Entities;

using Domain.Common.DomainEvents;

public abstract class EntityBase : IEntity
{
    private readonly IList<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        var events = _domainEvents.ToList().AsReadOnly();

        _domainEvents.Clear();

        return events;
    }

    protected internal void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
