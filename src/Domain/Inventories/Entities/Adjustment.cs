namespace Domain.Inventories.Entities;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;

public sealed class Adjustment : IChildEntity
{
    public AdjustmentId Id { get; init; }
    public InventoryId InventoryId { get; init; }
    public OrderId? OrderId { get; init; }
    public Quantity Quantity { get; init; }
    public string Reason { get; init; }
}
