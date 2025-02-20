namespace Infrastructure.Persistence.Products;

using Domain.Products.Entities;
using Infrastructure.Persistence.Inventories;
using Infrastructure.Persistence.Tables;

public static class ProductMapper
{
    public static IQueryable<Product> ToEntityQueryable(IQueryable<ProductTable> queryable)
    {
        return queryable.Select(table => ToEntity(table));
    }

    public static Product ToEntity(ProductTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return Product.Create(
            table.Name,
            table.Id,
            table.Inventory is null ? null : InventoryMapper.ToEntity(table.Inventory)
        );
    }

    public static ProductTable ToTable(Product entity)
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
