using Domain.Products.Aggregate;
using Infrastructure.Persistence.Tables;

namespace Infrastructure.Persistence.Repositories.Products;

internal static class ProductMapper
{
    internal static IQueryable<Product> ToEntityQueryable(IQueryable<ProductTable> queryable)
    {
        return queryable.Select(table => ToEntity(table));
    }

    internal static Product ToEntity(ProductTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        var productBuilder = ProductBuilder.Start().WithId(table.Id).WithName(table.Name);

        if (table.Inventory is not null)
            productBuilder.WithInventory(InventoryMapper.ToEntity(table.Inventory));

        return productBuilder.Build();
    }

    internal static ProductTable ToTable(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ProductTable
        {
            Id = entity.Id,
            Inventory = entity.HasInventory ? InventoryMapper.ToTable(entity.Inventory!) : null,
            Name = entity.Name,
        };
    }
}
