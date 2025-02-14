namespace Infrastructure.Persistence.Tables;

using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Products.ValueObjects;

public sealed class InventoryTable
{
    public InventoryId Id { get; set; }
    public ProductId ProductId { get; set; }
    public Quantity Quantity { get; set; }

    public ProductTable Product { get; set; }
    public ICollection<AdjustmentTable> Adjustments { get; set; }
    public ICollection<ReservationTable> Reservations { get; set; }
}
