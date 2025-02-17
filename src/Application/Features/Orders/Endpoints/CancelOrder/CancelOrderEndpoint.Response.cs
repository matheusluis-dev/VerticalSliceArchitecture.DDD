namespace Application.Features.Orders.Endpoints.CancelOrder;

using Domain.Orders.ValueObjects;

public sealed record Response(OrderId Id);
