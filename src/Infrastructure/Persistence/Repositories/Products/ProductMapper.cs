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

        return Product.Create(
            table.Name,
            table.Id,
            table.Inventory is null ? null : InventoryMapper.ToEntity(table.Inventory)
        );
    }

    internal static ProductTable ToTable(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ProductTable
        {
            Id = entity.Id,
            Inventory = entity.Inventory is null ? null : InventoryMapper.ToTable(entity.Inventory),
            Name = entity.Name,
        };
    }
}
