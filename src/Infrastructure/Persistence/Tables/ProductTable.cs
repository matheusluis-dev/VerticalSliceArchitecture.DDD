namespace Infrastructure.Persistence.Tables;

using System.Diagnostics.CodeAnalysis;
using Domain.Inventories.Ids;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ProductTable
{
    public required ProductId Id { get; set; }
    public InventoryId? InventoryId { get; set; }
    public ProductName Name { get; set; }

    public ICollection<OrderItemTable> OrderItems { get; set; }
    public InventoryTable? Inventory { get; set; }
}

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
