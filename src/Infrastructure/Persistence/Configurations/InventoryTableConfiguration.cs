namespace Infrastructure.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
