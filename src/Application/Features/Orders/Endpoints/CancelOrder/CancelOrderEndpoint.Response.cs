namespace Application.Features.Orders.Endpoints.CancelOrder;

using Domain.Orders.Ids;

public sealed record Response(OrderId Id);
