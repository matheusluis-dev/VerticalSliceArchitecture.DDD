namespace Application.Features.Orders.Endpoints.PlaceOrder;

using Domain.Orders.ValueObjects;

public sealed record Response(OrderId Id);
