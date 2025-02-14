namespace Domain.Products.Specifications;

using Domain.Common.Specifications;
using Domain.Products.Entities;

public sealed class HasInventorySpecification : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.Inventory is not null;
    }
}
