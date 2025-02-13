namespace Application.Domain.Products.Specifications;

using Application.Domain.__Common.Specifications;
using Application.Domain.Products.Entities;

public class ProductNameMustNotBeEmptySpecification : ISpecification<Product>
{
    public bool IsSatisfiedBy(Product entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return !entity.Name.IsNullOrWhiteSpace();
    }
}
