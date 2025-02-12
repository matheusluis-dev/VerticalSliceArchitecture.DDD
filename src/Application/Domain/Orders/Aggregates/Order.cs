namespace Application.Domain.Orders.Aggregates;

using System;
using System.Collections.Generic;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Entities;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.Models;
using Application.Domain.Orders.Services.UpdateOrder.Models;
using Application.Domain.Orders.ValueObjects;
using Application.Infrastructure.Services;
using Ardalis.Result;
using LinqKit;

public sealed class Order : IAggregate
{
    public required OrderId Id { get; init; }

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyList<OrderItem> OrderItems
    {
        get => _orderItems.AsReadOnly();
        init => _orderItems = [.. value];
    }
    public required OrderStatus Status { get; init; }
    public required Email CustomerEmail { get; init; }
    public required DateTime CreatedDate { get; init; }
    public DateTime? PaidDate { get; init; }
    public DateTime? CanceledDate { get; init; }

    public static Order Create(
        IDateTimeService dateTime,
        IEnumerable<AddOrderItemModel> items,
        Email customerEmail
    )
    {
        ArgumentNullException.ThrowIfNull(dateTime);
        ArgumentNullException.ThrowIfNull(items);

        if (!items.Any())
            throw new Exception("TODO");

        var order = new Order
        {
            Id = OrderId.Create(),
            Status = OrderStatus.Pending,
            CustomerEmail = customerEmail,
            CreatedDate = dateTime.UtcNow.DateTime,
            CanceledDate = null,
            PaidDate = null,
        };

        items.ForEach(item => order.AddItem(item));

        return order;
    }

    public Amount GetTotalPrice()
    {
        return Amount.From(OrderItems.Sum(item => item.Quantity.Value * item.UnitPrice.Value));
    }

    public Result<OrderItem> AddItem(AddOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (OrderItems.Any(item => item.ProductId == model.ProductId))
            throw new Exception("TODO");

        var item = new OrderItem
        {
            Id = OrderItemId.Create(),
            OrderId = Id,
            ProductId = model.ProductId,
            Quantity = model.Quantity,
            UnitPrice = model.UnitPrice,
        };
        _orderItems.Add(item);

        return Result.Created(item);
    }

    public Result<OrderItem> UpdateItem(UpdateOrderItemModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var id = model.Id;
        var item = OrderItems.FirstOrDefault(item => item.Id == id);

        if (item is null)
            return Result.NotFound($"Item with ID {model.Id} not found");

        var itemIndex = _orderItems.IndexOf(item);

        _orderItems[itemIndex] = new OrderItem
        {
            OrderId = item.OrderId,
            Id = model.Id,
            Quantity = model.Quantity ?? item.Quantity,
            UnitPrice = model.UnitPrice ?? item.UnitPrice,
            ProductId = item.ProductId,
            ReservationId = item.ReservationId,
        };

        return Result.Success(item);
    }

    public Result DeleteItem(OrderItemId itemId)
    {
        var item = _orderItems.FirstOrDefault(item => item.Id == itemId);

        if (item is null)
            return Result.NotFound($"Item with ID {itemId} not found");

        _orderItems.Remove(item);

        return Result.Success();
    }
}
