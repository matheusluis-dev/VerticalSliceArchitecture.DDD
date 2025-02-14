namespace Infrastructure.Persistence.Products;

using Domain.Products.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Inventories;
using Infrastructure.Persistence.Tables;

public sealed class ProductMapper : IMapperWithQueryable<Product, ProductTable>
{
    private readonly InventoryMapper _inventoryMapper;

    public ProductMapper(InventoryMapper inventoryMapper)
    {
        _inventoryMapper = inventoryMapper;
    }

    public IQueryable<Product> ToEntityQueryable(IQueryable<ProductTable> queryable)
    {
        return queryable.Select(table => ToEntity(table));
    }

    public Product ToEntity(ProductTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        return new()
        {
            Id = table.Id,
            Inventory = table.Inventory is null ? null : _inventoryMapper.ToEntity(table.Inventory),
            Name = table.Name,
        };
    }

    public ProductTable ToTable(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ProductTable
        {
            Id = entity.Id,
            Inventory = entity.Inventory is null
                ? null
                : _inventoryMapper.ToTable(entity.Inventory),
            Name = entity.Name,
        };
    }
}
