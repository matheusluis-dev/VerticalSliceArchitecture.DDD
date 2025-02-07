namespace Application.Infrastructure.Persistence.Orders.Mappers;

using System.Diagnostics.CodeAnalysis;
using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Entities;
using Application.Infrastructure.Persistence.Orders.Tables;

public static class OrderMapper
{
    public static IQueryable<Order> ToEntityQueryable(
        [NotNull] this IQueryable<OrderTable> orderModel
    )
    {
        return orderModel.Select(o => o.ToEntity());
    }

    public static Order ToEntity([NotNull] this OrderTable orderModel)
    {
        return new Order
        {
            Id = orderModel.Id,
            Status = orderModel.Status,
            OrderItems = orderModel.OrderItems.Select(i => i.ToEntity()).ToList(),
        };
    }

    public static OrderItem ToEntity([NotNull] this OrderItemTable orderItemModel)
    {
        return new OrderItem
        {
            Id = orderItemModel.Id,
            OrderId = orderItemModel.OrderId,
            Quantity = orderItemModel.Quantity,
            UnitPrice = orderItemModel.UnitPrice,
            Created = orderItemModel.Created,
            CreatedBy = orderItemModel.CreatedBy,
            LastModified = orderItemModel.LastModified,
            LastModifiedBy = orderItemModel.LastModifiedBy,
        };
    }

    public static OrderTable FromEntity([NotNull] this Order order)
    {
        return new OrderTable
        {
            Id = order.Id,
            Status = order.Status,
            OrderItems = order.OrderItems.Select(i => i.FromEntity()).ToList(),
        };
    }

    public static OrderItemTable FromEntity([NotNull] this OrderItem orderItem)
    {
        return new OrderItemTable
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            Created = orderItem.Created,
            CreatedBy = orderItem.CreatedBy,
            LastModified = orderItem.LastModified,
            LastModifiedBy = orderItem.LastModifiedBy,
        };
    }
}
