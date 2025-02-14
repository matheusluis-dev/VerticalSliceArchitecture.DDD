namespace Application.Features.Orders.PlaceOrder;

using System.ComponentModel.DataAnnotations;
using Domain.Orders.ValueObjects;

public sealed record Response([Required] OrderId Id);
