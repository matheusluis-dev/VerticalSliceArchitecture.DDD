using Domain.Products.Aggregate;

namespace Domain.Products.Events;

public sealed record InventoryStockReachedZeroEvent(Product Product) : IDomainEvent;
