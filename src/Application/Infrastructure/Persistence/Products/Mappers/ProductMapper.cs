namespace Application.Infrastructure.Persistence.Products.Mappers;

using System.Diagnostics.CodeAnalysis;
using Application.Domain.Products.Entities;
using Application.Infrastructure.Persistence.Products.Tables;

public static class ProductMapper
{
    public static IQueryable<Product> ToEntityQueryable(
        [NotNull] this IQueryable<ProductTable> queryable
    )
    {
        return queryable.Select(t => t.ToEntity());
    }

    public static Product ToEntity([NotNull] this ProductTable table)
    {
        return new Product
        {
            Id = table.Id,
            InventoryId = table.InventoryId,
            Name = table.Name,
            Created = table.Created,
            CreatedBy = table.CreatedBy,
            LastModified = table.LastModified,
            LastModifiedBy = table.LastModifiedBy,
        };
    }

    public static ProductTable FromEntity([NotNull] this Product entity)
    {
        return new ProductTable
        {
            Id = entity.Id,
            InventoryId = entity.InventoryId,
            Name = entity.Name,
            Created = entity.Created,
            CreatedBy = entity.CreatedBy,
            LastModified = entity.LastModified,
            LastModifiedBy = entity.LastModifiedBy,
        };
    }
}
