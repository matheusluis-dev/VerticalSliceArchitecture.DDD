namespace Application.Infrastructure.Persistence.Orders.Tables;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Enums;
using Application.Domain.Orders.ValueObjects;
using Application.Domain.User.ValueObjects;

public sealed class OrderTable : IAuditable
{
    public OrderId Id { get; set; }
    public ICollection<OrderItemTable> OrderItems { get; set; } = [];
    public Amount TotalPrice { get; set; }
    public OrderStatus Status { get; set; }

    public DateTime Created { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserId LastModifiedBy { get; set; }
}
