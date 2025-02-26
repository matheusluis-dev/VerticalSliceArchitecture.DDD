using Domain.Orders.Aggregates;

namespace Domain.Orders.Events;

public sealed record OrderPaidEvent(Order Order) : IDomainEvent;
