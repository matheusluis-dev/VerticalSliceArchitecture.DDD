namespace Application.Infrastructure.Persistence.Orders.Tables;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.ValueObjects;
using Application.Domain.Products.ValueObjects;
using Application.Domain.User.ValueObjects;
using Application.Infrastructure.Persistence.Products.Tables;

public sealed class OrderItemTable : IAuditable
{
    public OrderId OrderId { get; set; }
    public OrderItemId Id { get; set; }
    public ProductId ProductId { get; set; }
    public Quantity Quantity { get; set; }
    public Amount UnitPrice { get; set; }
    public Amount Price { get; set; }

    public DateTime Created { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserId LastModifiedBy { get; set; }

    public OrderTable Order { get; set; }
    public ProductTable Product { get; set; }
}
