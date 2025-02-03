namespace Application.Features.Orders.CreateOrder;

using System.ComponentModel.DataAnnotations;
using Application.Domain.Orders.ValueObjects;

public static partial class CreateOrderEndpoint
{
    public sealed record Response([Required] OrderId Id);
}
