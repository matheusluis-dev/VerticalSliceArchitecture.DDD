namespace Application.Endpoints.Orders.DeleteOrder;

using Application.Domain.Orders.ValueObjects;

public sealed record Request(OrderId Id);
