namespace Application.Features.Orders.GetOrdersPaged;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Enums;
using static Application.Features.Orders.GetOrdersPaged.GetOrdersPagedEndpoint.Response;

public static partial class GetOrdersPagedEndpoint
{
    public sealed record Response(IEnumerable<OrderResponse> Orders, int PageIndex, int TotalPages)
    {
        public sealed record OrderResponse(
            OrderStatus Status,
            Amount TotalPrice,
            IEnumerable<OrderItemResponse> Items
        );

        public sealed record OrderItemResponse(Quantity Quantity, Amount UnitPrice);
    }
}
