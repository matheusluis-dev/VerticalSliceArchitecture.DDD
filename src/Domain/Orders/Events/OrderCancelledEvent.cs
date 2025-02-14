namespace Domain.Orders.Events;

using Domain.Common.DomainEvents;
using Domain.Orders.Aggregates;

public sealed record OrderCancelledEvent(Order Order) : IDomainEvent;
