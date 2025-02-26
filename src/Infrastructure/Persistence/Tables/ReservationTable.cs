using System.Diagnostics.CodeAnalysis;
using Domain.Common.ValueObjects;
using Domain.Inventories.Enums;
using Domain.Inventories.Ids;
using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

#nullable disable

public sealed class ReservationTable
{
    public ReservationId Id { get; set; }
    public InventoryId InventoryId { get; set; }
    public OrderItemId OrderItemId { get; set; }
    public Quantity Quantity { get; set; }
    public ReservationStatus Status { get; set; }

    public OrderItemTable OrderItem { get; set; }
    public InventoryTable Inventory { get; set; }
}

#nullable  restore

public sealed class ReservationTableConfiguration : IEntityTypeConfiguration<ReservationTable>
{
    public void Configure([NotNull] EntityTypeBuilder<ReservationTable> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Id);
    }
}
