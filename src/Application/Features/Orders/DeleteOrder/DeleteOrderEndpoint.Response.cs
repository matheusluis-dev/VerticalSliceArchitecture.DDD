namespace Application.Features.Orders.DeleteOrder;

using Application.Domain.Orders.ValueObjects;

public static partial class DeleteOrderEndpoint
{
    public sealed record Response(OrderId Id);
}
