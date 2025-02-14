namespace Application.Features.Orders.DeleteOrder;

using Domain.Orders.ValueObjects;

public sealed record Request(OrderId Id);
