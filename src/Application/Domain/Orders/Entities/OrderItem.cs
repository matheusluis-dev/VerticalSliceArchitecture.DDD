namespace Application.Domain.Orders.Entities;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class OrderItem : IChildEntity, IAuditable
{
    public required OrderItemId Id { get; init; }
    public required OrderId OrderId { get; init; }
    public Quantity Quantity { get; init; }
    public Amount UnitPrice { get; init; }
    public Amount Price => UnitPrice.MultiplyByQuantity(Quantity);

    public DateTime Created { get; set; }
    public UserName CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserName LastModifiedBy { get; set; }
}
