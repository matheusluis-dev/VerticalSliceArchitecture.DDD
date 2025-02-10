namespace Application.Infrastructure.Persistence.Products.Tables;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Products.ValueObjects;
using Application.Domain.User.ValueObjects;
using Application.Infrastructure.Persistence.Inventories.Tables;
using Application.Infrastructure.Persistence.Orders.Tables;

public sealed class ProductTable : IAuditable
{
    public required ProductId Id { get; set; }
    public InventoryId? InventoryId { get; set; }
    public ProductName Name { get; set; }

    public DateTime Created { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserId LastModifiedBy { get; set; }

    public ICollection<OrderItemTable> OrderItems { get; set; }
    public InventoryTable Inventory { get; set; }
}
