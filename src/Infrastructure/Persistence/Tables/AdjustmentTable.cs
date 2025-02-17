namespace Infrastructure.Persistence.Tables;

using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;

public sealed class AdjustmentTable
{
    public AdjustmentId Id { get; set; }
    public InventoryId InventoryId { get; set; }
    public OrderItemId? OrderItemId { get; set; }
    public Quantity Quantity { get; set; }
    public string Reason { get; set; }

    public OrderTable? Order { get; set; }
    public InventoryTable Inventory { get; set; }
}
