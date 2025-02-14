namespace Application.Endpoints.Orders.GetOrdersPaged;

using Domain.Common.ValueObjects;
using Domain.Orders.Enums;
using static Application.Endpoints.Orders.GetOrdersPaged.GetOrdersPagedEndpoint.Response;

public static partial class GetOrdersPagedEndpoint
{
    public sealed record Response(
        int PageIndex,
        int TotalPages,
        bool HasPreviousPage,
        bool HasNextPage,
        IEnumerable<OrderResponse> Orders
    )
    {
        public sealed record OrderResponse
        {
            public required OrderStatus Status { get; init; }
            public required Amount TotalPrice { get; init; }
            public required IEnumerable<OrderItemResponse> Items { get; init; }
        }

        public sealed record OrderItemResponse
        {
            public required Quantity Quantity { get; init; }
            public required Amount UnitPrice { get; init; }
        }
    }
}
