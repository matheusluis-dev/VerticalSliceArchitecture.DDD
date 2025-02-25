namespace Infrastructure.Persistence.Tables;

using System.Diagnostics.CodeAnalysis;
using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;
using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class InventoryTable
{
    public InventoryId Id { get; set; }
    public ProductId ProductId { get; set; }
    public Quantity Quantity { get; set; }

    public ProductTable Product { get; set; }
    public ICollection<AdjustmentTable> Adjustments { get; set; }
    public ICollection<ReservationTable> Reservations { get; set; }
}

public sealed class InventoryTableConfiguration : IEntityTypeConfiguration<InventoryTable>
{
    public void Configure([NotNull] EntityTypeBuilder<InventoryTable> builder)
    {
        builder.HasKey(inventory => inventory.Id);
        builder.HasIndex(inventory => inventory.Id);

        builder
            .HasOne(inventory => inventory.Product)
            .WithOne(product => product.Inventory)
            .HasForeignKey<InventoryTable>(inventory => inventory.ProductId)
            .IsRequired(true)
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
