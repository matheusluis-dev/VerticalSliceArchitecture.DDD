namespace Application.Features.Orders.Endpoints.PayOrder;

using Domain.Orders.Ids;

public sealed record Request(OrderId Id);
