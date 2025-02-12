namespace Application.Infrastructure.Persistence.Products;

using Application.Domain.Products.Entities;
using Application.Infrastructure.Persistence.Tables;

public sealed class ProductMapper : IMapperWithQueryable<Product, ProductTable>
{
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
            InventoryId = table.InventoryId,
            Name = table.Name,
        };
    }

    public ProductTable ToTable(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ProductTable
        {
            Id = entity.Id,
            InventoryId = entity.InventoryId,
            Name = entity.Name,
        };
    }
}
