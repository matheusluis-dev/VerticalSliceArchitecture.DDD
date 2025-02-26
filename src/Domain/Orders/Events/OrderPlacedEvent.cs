using Domain.Orders.Aggregates;

namespace Domain.Orders.Events;

public sealed record OrderPlacedEvent(Order Order) : IDomainEvent;
