using Domain.Inventories.Ids;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Tables;

public sealed class ProductTable
{
    public required ProductId Id { get; set; }
    public InventoryId? InventoryId { get; set; }
    public required ProductName Name { get; set; }

    public ICollection<OrderItemTable>? OrderItems { get; init; } = [];
    public InventoryTable? Inventory { get; set; }
}

public sealed class ProductTableConfiguration : IEntityTypeConfiguration<ProductTable>
{
    public void Configure(EntityTypeBuilder<ProductTable> builder)
    {
        builder.HasKey(product => product.Id);
        builder.HasIndex(product => product.Id);

        builder.HasIndex(m => m.Name).IsUnique();

        builder.Property(p => p.Name).HasConversion<ProductNameConverter>();

        builder
            .HasOne(product => product.Inventory)
            .WithOne(inventory => inventory.Product)
            .HasForeignKey<InventoryTable>(inventory => inventory.ProductId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ProductNameConverter : ValueConverter<ProductName, string>
{
    public ProductNameConverter()
        : base(productName => productName.Value, @string => new ProductName(@string)) { }
}
