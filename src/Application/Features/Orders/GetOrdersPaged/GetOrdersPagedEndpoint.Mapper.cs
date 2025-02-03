namespace Application.Features.Orders.GetOrdersPaged;

using System.Threading;
using Application.Domain.Common;
using Application.Domain.Common.ValueObjects;
using Ardalis.GuardClauses;
using FastEndpoints;

public static partial class GetOrdersPagedEndpoint
{
    public sealed class PagedOrderMapper
        : Mapper<Request, Response, IPagedList<Domain.Orders.Aggregates.Order>>
    {
        public override async Task<Response> FromEntityAsync(
            IPagedList<Domain.Orders.Aggregates.Order> e,
            CancellationToken ct = default
        )
        {
            Guard.Against.Null(e);

            var orders = await e.GetListAsync(
                query =>
                    query.Select(order => new Response.OrderResponse
                    {
                        Status = order.Status,
                        TotalPrice = order.TotalPrice,
                        Items = order.OrderItems.AsQueryable().Select(item => new Response.OrderItemResponse
                        {
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                        }),
                    }),
                ct
            );

            return new Response(orders, e.PageIndex, e.TotalPages);
        }
    }
}
