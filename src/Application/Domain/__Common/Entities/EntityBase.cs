namespace Application.Domain.__Common.Entities;

using Application.Domain.__Common.DomainEvents;

public abstract class EntityBase : IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        var events = _domainEvents.ToList().AsReadOnly();

        _domainEvents.Clear();

        return events;
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
