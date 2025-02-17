namespace Domain.Inventories.Events;

using Domain.Common.DomainEvents;
using Domain.Inventories.ValueObjects;
using Domain.Products.ValueObjects;

public sealed record InventoryStockReachedZeroEvent(InventoryId InventoryId, ProductId ProductId)
    : IDomainEvent;
