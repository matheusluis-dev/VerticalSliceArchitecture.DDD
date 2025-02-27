using Domain.Common.Specifications;
using Domain.Products.Entities;

namespace Domain.Products.Specifications;

public sealed class ProductCanBeDeletedSpecification : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.Inventory is null)
            return true;

        return entity.Inventory.Adjustments.Count == 0 && entity.Inventory.Reservations.Count == 0;
    }
}
