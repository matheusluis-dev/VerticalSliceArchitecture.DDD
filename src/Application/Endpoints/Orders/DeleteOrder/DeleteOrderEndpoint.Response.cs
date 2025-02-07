namespace Application.Endpoints.Orders.DeleteOrder;

using Application.Domain.Orders.ValueObjects;

public sealed record Response(OrderId Id);
