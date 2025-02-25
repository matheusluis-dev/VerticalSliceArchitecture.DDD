namespace Domain.Inventories.Events;

using Domain.Common.DomainEvents;
using Domain.Inventories.Aggregate;
using Domain.Products.Ids;

public sealed record InventoryStockReachedZeroEvent(Inventory Inventory, ProductId ProductId) : IDomainEvent;
