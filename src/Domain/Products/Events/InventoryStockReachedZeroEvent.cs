using Domain.Products.Entities;

namespace Domain.Products.Events;

public sealed record InventoryStockReachedZeroEvent(Inventory Inventory, ProductId ProductId) : IDomainEvent;
