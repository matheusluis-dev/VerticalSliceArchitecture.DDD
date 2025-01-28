using Application.Domain.Orders.Entities;

namespace Application.Domain.Orders.Aggregates;

using System;
using System.Collections.Generic;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.Services.UpdateOrder.Models;
using Application.Domain.Orders.ValueObjects;
using Ardalis.GuardClauses;
using Ardalis.Result;

public sealed class Order : IAggregate, IAuditable
{
    public required OrderId Id { get; init; }
    public IList<OrderItem> OrderItems { get; init; } = [];
    public OrderStatus Status { get; set; }
    public Amount TotalPrice => Amount.From(OrderItems.Sum(order => order.Price.Value));

    public DateTime Created { get; set; }
    public UserName CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserName LastModifiedBy { get; set; }

    public Result<OrderItem> AddOrderItem(OrderItem newItem)
    {
        Guard.Against.Null(newItem);

        if (OrderItems.Select(item => item.Id).Contains(newItem.Id))
            return Result.Error($"Item with ID {newItem.Id} already exists.");

        OrderItems.Add(newItem);

        return Result.Created(newItem);
    }

    public Result<OrderItem> UpdateOrderItem(UpdateOrderItemModel model)
    {
        Guard.Against.Null(model);

        var id = model.Id;
        var item = OrderItems.FirstOrDefault(item => item.Id == id);

        if (item is null)
            return Result.NotFound($"Item with ID {model.Id} not found");

        var itemIndex = OrderItems.IndexOf(item);

        OrderItems[itemIndex] = new OrderItem
        {
            OrderId = item.OrderId,
            Id = model.Id,
            Quantity = model.Quantity ?? item.Quantity,
            UnitPrice = model.UnitPrice ?? item.UnitPrice,
        };

        return Result.Success(item);
    }

    public Result DeleteOrderItem(OrderItemId itemId)
    {
        Guard.Against.Null(itemId);

        var item = OrderItems.FirstOrDefault(item => item.Id == itemId);

        if (item is null)
            return Result.NotFound($"Item with ID {itemId} not found");

        OrderItems.Remove(item);

        return Result.Success();
    }
}
