namespace Application.Infrastructure.Persistence.Tables;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class ReservationTable
{
    public ReservationId Id { get; set; }
    public InventoryId InventoryId { get; set; }
    public OrderItemId OrderItemId { get; set; }
    public Quantity Quantity { get; set; }

    public OrderItemTable OrderItem { get; set; }
    public InventoryTable Inventory { get; set; }
}
