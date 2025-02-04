namespace Application.Features.Orders.CreateOrder;

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.ValueObjects;
using FastEndpoints;

public static partial class CreateOrderEndpoint
{
    public sealed class OrderMapper : Mapper<Request, Response, Domain.Orders.Aggregates.Order>
    {
        public override Task<Response> FromEntityAsync(
            [NotNull] Domain.Orders.Aggregates.Order e,
            CancellationToken ct = default
        )
        {
            return Task.FromResult(new Response(e.Id));
        }

        public override Task<Domain.Orders.Aggregates.Order> ToEntityAsync(
            Request r,
            CancellationToken ct = default
        )
        {
            var id = OrderId.Create();

            var orderItems =
                r?.Items?.Select(item => new OrderItem
                    {
                        Id = OrderItemId.Create(),
                        OrderId = id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    })
                    .ToList() ?? [];

            return Task.FromResult(
                new Domain.Orders.Aggregates.Order
                {
                    Id = id,
                    Status = OrderStatus.Pending,
                    OrderItems = orderItems,
                }
            );
        }
    }
}
