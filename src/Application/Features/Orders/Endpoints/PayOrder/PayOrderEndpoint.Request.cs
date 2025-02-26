using Domain.Orders.Ids;

namespace Application.Features.Orders.Endpoints.PayOrder;

public sealed record Request(OrderId Id);
