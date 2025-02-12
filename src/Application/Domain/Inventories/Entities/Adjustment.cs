namespace Application.Domain.Inventories.Entities;

using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class Adjustment : IChildEntity
{
    public AdjustmentId Id { get; init; }
    public InventoryId InventoryId { get; init; }
    public OrderId? OrderId { get; init; }
    public Quantity Quantity { get; init; }
    public string Reason { get; init; }
}
