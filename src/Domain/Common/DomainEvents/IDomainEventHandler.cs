namespace Domain.Common.DomainEvents;

public interface IDomainEventHandler<TEvent> : FastEndpoints.IEventHandler<TEvent>
    where TEvent : class, IDomainEvent;
