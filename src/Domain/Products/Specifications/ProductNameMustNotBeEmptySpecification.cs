namespace Domain.Products.Specifications;

using Domain.Common.Specifications;
using Domain.Products.Entities;

public class ProductNameMustNotBeEmptySpecification : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !entity.Name.IsNullOrWhiteSpace();
    }
}
