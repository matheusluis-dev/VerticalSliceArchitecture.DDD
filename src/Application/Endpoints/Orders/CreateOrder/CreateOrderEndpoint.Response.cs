namespace Application.Endpoints.Orders.CreateOrder;

using System.ComponentModel.DataAnnotations;
using Application.Domain.Orders.ValueObjects;

public sealed record Response([Required] OrderId Id);
