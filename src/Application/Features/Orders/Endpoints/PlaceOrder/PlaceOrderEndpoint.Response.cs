namespace Application.Features.Orders.Endpoints.PlaceOrder;

using Domain.Orders.Ids;

public sealed record Response(OrderId Id);
