namespace Application.Features.Orders.Endpoints.PayOrder;

using Domain.Orders.ValueObjects;

public sealed record Request(OrderId Id);
