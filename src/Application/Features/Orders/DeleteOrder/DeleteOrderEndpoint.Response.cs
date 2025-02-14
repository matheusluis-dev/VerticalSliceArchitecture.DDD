namespace Application.Features.Orders.DeleteOrder;

using Domain.Orders.ValueObjects;

public sealed record Response(OrderId Id);
