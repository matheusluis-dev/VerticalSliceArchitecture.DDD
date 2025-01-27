namespace Application.Infrastructure.Order.Models;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.ValueObjects;

public sealed class OrderModel : IAuditable
{
    public OrderId Id { get; set; }
    public Amount TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<OrderItemModel> OrderItems { get; set; } = [];

    public DateTime Created { get; set; }
    public UserName CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserName LastModifiedBy { get; set; }
}
