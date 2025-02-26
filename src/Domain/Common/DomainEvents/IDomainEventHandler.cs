namespace Domain.Common.DomainEvents;

#pragma warning disable CA1711

public interface IDomainEventHandler<TEvent> : FastEndpoints.IEventHandler<TEvent>
    where TEvent : class, IDomainEvent;

#pragma warning restore CA1711
