using Domain.Inventories.Aggregate;

namespace Domain.Inventories.Events;

public sealed record InventoryStockReachedZeroEvent(Inventory Inventory, ProductId ProductId) : IDomainEvent;
