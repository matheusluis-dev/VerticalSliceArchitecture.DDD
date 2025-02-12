namespace Application.Infrastructure.Persistence.Configurations;

using System.Diagnostics.CodeAnalysis;
using Application.Infrastructure.Persistence.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ProductTableConfiguration : IEntityTypeConfiguration<ProductTable>
{
    public void Configure([NotNull] EntityTypeBuilder<ProductTable> builder)
    {
        builder.HasKey(product => product.Id);
        builder.HasIndex(product => product.Id);

        builder.HasIndex(m => m.Name).IsUnique(true);

        builder
            .HasOne(product => product.Inventory)
            .WithOne(inventory => inventory.Product)
            .HasForeignKey<InventoryTable>(inventory => inventory.ProductId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
