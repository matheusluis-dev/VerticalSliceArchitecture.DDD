using Domain.Common.ValueObjects;
using Domain.Orders.Ids;
using Domain.Products.Enums;
using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

public sealed class ReservationTable
{
    public required ReservationId Id { get; set; }
    public required InventoryId InventoryId { get; set; }
    public required OrderItemId OrderItemId { get; set; }
    public required Quantity Quantity { get; set; }
    public required ReservationStatus Status { get; set; }

    public OrderItemTable? OrderItem { get; set; }
    public InventoryTable? Inventory { get; set; }
}

public sealed class ReservationTableConfiguration : IEntityTypeConfiguration<ReservationTable>
{
    public void Configure(EntityTypeBuilder<ReservationTable> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id);
    }
}
