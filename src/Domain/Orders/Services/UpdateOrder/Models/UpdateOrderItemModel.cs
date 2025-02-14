namespace Domain.Orders.Services.UpdateOrder.Models;

using Domain.Common.ValueObjects;
using Domain.Orders.ValueObjects;

public sealed record UpdateOrderItemModel(OrderItemId Id, Quantity? Quantity, Amount? UnitPrice);
