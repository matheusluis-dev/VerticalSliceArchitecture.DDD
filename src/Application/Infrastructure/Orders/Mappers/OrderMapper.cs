namespace Application.Infrastructure.Orders.Mappers;

using Application.Domain.Orders.Aggregates;
using Application.Domain.Orders.Entities;
using Application.Infrastructure.Orders.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class OrderMapper
{
    public static partial IQueryable<Order> ToEntityQueryable(
        this IQueryable<OrderModel> orderModel
    );

    [MapProperty(nameof(Order.OrderItems), nameof(OrderModel.OrderItems))]
    public static partial OrderModel ToModel(this Order order);

    [MapProperty(nameof(OrderItem.OrderId), nameof(OrderItemModel.Order.Id))]
    public static partial OrderItemModel ToItemModel(this OrderItem item);

    public static partial IEnumerable<OrderItemModel> ToModelEnumerable(
        this IEnumerable<OrderItem> item
    );
}
