using Domain.Orders.Ids;

namespace Application.Features.Orders.Endpoints.PlaceOrder;

public sealed record Response(OrderId Id);
