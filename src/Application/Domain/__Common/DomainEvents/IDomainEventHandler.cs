namespace Application.Domain.__Common.DomainEvents;

public interface IDomainEventHandler<TEvent> : IEventHandler<TEvent>
    where TEvent : class, IDomainEvent;
