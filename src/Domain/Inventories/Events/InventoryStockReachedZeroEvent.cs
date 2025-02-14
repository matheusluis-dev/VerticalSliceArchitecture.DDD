namespace Domain.Inventories.Events;

using Domain.Common.DomainEvents;
using Domain.Inventories.ValueObjects;

public sealed record InventoryStockReachedZeroEvent(InventoryId Id) : IDomainEvent;
