namespace Application.Features.Orders.CreateOrder;

using System.ComponentModel.DataAnnotations;
using Application.Domain.Common.ValueObjects;
using static Application.Features.Orders.CreateOrder.CreateOrderEndpoint.Request;

public static partial class CreateOrderEndpoint
{
    public sealed record Request(IEnumerable<RequestItems>? Items)
    {
        public sealed record RequestItems(
            [Required] Quantity Quantity,
            [Required] Amount UnitPrice
        );
    }
}
