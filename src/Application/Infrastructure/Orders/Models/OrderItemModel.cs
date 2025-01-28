namespace Application.Infrastructure.Orders.Models;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class OrderItemModel : IAuditable
{
    public OrderItemId Id { get; set; }
    public required OrderModel Order { get; set; }
    public Quantity Quantity { get; set; }
    public Amount UnitPrice { get; set; }
    public Amount Price { get; set; }

    public DateTime Created { get; set; }
    public UserName CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserName LastModifiedBy { get; set; }
}
