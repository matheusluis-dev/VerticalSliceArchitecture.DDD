namespace Application.Features.Orders.GetOrdersPaged;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Enums;
using static Application.Features.Orders.GetOrdersPaged.GetOrdersPagedEndpoint.Response;

public static partial class GetOrdersPagedEndpoint
{
    public sealed record Response(IEnumerable<OrderResponse> Orders, int PageIndex, int TotalPages)
    {
        //public required IEnumerable<OrderResponse> Orders { get; init; }
        //public required int PageIndex { get; init; }
        //public required int TotalPages { get; init; }

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
