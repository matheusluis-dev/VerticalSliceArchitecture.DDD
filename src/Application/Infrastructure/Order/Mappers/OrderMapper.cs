namespace Application.Infrastructure.Order.Mappers;

using Application.Domain.Orders.Entities;
using Application.Infrastructure.Order.Models;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class OrderMapper
{
    public static partial IQueryable<Order> ToQueryableEntity(
        this IQueryable<OrderModel> orderModel
    );
}
