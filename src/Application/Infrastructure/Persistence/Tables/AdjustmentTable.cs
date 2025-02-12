namespace Application.Infrastructure.Persistence.Tables;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class AdjustmentTable
{
    public AdjustmentId Id { get; set; }
    public InventoryId InventoryId { get; set; }
    public OrderId? OrderId { get; set; }
    public Quantity Quantity { get; set; }
    public string Reason { get; set; }

    public OrderTable? Order { get; set; }
    public InventoryTable Inventory { get; set; }
}
