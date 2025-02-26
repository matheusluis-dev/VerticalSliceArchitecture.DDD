using Domain.Orders.Aggregates;

namespace Domain.Orders.Events;

public sealed record OrderCancelledEvent(Order Order) : IDomainEvent;
