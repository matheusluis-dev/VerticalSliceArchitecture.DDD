namespace Application.Endpoints.Orders.GetOrdersPaged;

using System.Threading;
using System.Threading.Tasks;
using Application.Domain.Common;
using Ardalis.GuardClauses;
using FastEndpoints;

public static partial class GetOrdersPagedEndpoint
{
    public sealed class PagedOrderMapper
        : Mapper<Request, Response, IPagedList<Domain.Orders.Aggregates.Order>>
    {
        public override Task<Response> FromEntityAsync(
            IPagedList<Domain.Orders.Aggregates.Order> e,
            CancellationToken ct = default
        )
        {
            Guard.Against.Null(e);

            var orders = e.Elements.Select(order => new Response.OrderResponse
            {
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                Items = order.OrderItems.Select(item => new Response.OrderItemResponse
                {
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                }),
            });

            return Task.FromResult(
                new Response(e.PageIndex, e.TotalPages, e.HasPreviousPage, e.HasNextPage, orders)
            );
        }
    }
}
