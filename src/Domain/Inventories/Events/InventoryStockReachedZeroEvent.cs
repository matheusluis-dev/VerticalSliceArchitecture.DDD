namespace Domain.Inventories.Events;

using Domain.Common.DomainEvents;
using Domain.Inventories.Aggregate;
using Domain.Products.ValueObjects;

public sealed record InventoryStockReachedZeroEvent(Inventory Inventory, ProductId ProductId) : IDomainEvent;
