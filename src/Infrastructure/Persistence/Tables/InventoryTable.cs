using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;
using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Tables;

public sealed class InventoryTable
{
    public required InventoryId Id { get; set; }
    public required ProductId ProductId { get; set; }
    public required Quantity Quantity { get; set; }

    public ProductTable? Product { get; set; }
    public ICollection<AdjustmentTable> Adjustments { get; init; } = [];
    public ICollection<ReservationTable> Reservations { get; init; } = [];
}

public sealed class InventoryTableConfiguration : IEntityTypeConfiguration<InventoryTable>
{
    public void Configure(EntityTypeBuilder<InventoryTable> builder)
    {
        builder.HasKey(inventory => inventory.Id);
        builder.HasIndex(inventory => inventory.Id);

        builder
            .HasOne(inventory => inventory.Product)
            .WithOne(product => product.Inventory)
            .HasForeignKey<InventoryTable>(inventory => inventory.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(inventory => inventory.Adjustments)
            .WithOne(adjustment => adjustment.Inventory)
            .HasForeignKey(adjustment => adjustment.InventoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(inventory => inventory.Reservations)
            .WithOne(reservation => reservation.Inventory)
            .HasForeignKey(reservation => reservation.InventoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
