namespace Application.Infrastructure.Persistence.Orders.Tables;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class OrderItemTable : IAuditable
{
    public OrderItemId Id { get; set; }
    public OrderId OrderId { get; set; }
    public OrderTable Order { get; set; }
    public Quantity Quantity { get; set; }
    public Amount UnitPrice { get; set; }
    public Amount Price { get; set; }

    public DateTime Created { get; set; }
    public UserName CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserName LastModifiedBy { get; set; }
}
