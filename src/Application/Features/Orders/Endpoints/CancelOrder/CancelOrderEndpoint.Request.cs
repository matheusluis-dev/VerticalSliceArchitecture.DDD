using Domain.Orders.Ids;

namespace Application.Features.Orders.Endpoints.CancelOrder;

public sealed record Request(OrderId Id);
