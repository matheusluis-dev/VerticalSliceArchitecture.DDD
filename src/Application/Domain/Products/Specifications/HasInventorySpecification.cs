namespace Application.Domain.Products.Specifications;

using Application.Domain.__Common.Specifications;
using Application.Domain.Products.Entities;

public sealed class HasInventorySpecification : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.InventoryId is not null;
    }
}
