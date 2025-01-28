namespace Application.Domain.Orders.Services.UpdateOrder.Models;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed record UpdateOrderItemModel(OrderItemId Id, Quantity? Quantity, Amount? UnitPrice);
