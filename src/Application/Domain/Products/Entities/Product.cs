namespace Application.Domain.Products.Entities;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Products.ValueObjects;
using Application.Domain.User.ValueObjects;

public sealed class Product : IEntity, IAuditable
{
    public required ProductId Id { get; init; }
    public InventoryId InventoryId { get; init; } = InventoryId.From(Guid.Empty);
    public required ProductName Name { get; init; }

    public DateTime Created { get; set; }
    public UserId CreatedBy { get; set; } = UserId.From(Guid.Empty);
    public DateTime? LastModified { get; set; }
    public UserId LastModifiedBy { get; set; } = UserId.From(Guid.Empty);
}
