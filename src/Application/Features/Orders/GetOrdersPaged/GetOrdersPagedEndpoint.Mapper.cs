namespace Application.Features.Orders.GetOrdersPaged;

using Application.Domain.Common;
using FastEndpoints;

public static partial class GetOrdersPagedEndpoint
{
    public sealed class PagedOrderMapper
        : Mapper<Request, Response, IPagedList<Domain.Orders.Aggregates.Order>>
    {
        public override Response FromEntity(IPagedList<Domain.Orders.Aggregates.Order> e)
        {
            var orders = e.Queryable.Select(o => new Response.OrderResponse(
                o.Status,
                o.TotalPrice,
                o.OrderItems.Select(i => new Response.OrderItemResponse(i.Quantity, i.UnitPrice))
            )).;

            return new Response(orders, e.PageIndex, e.TotalPages);
        }
    }
}
