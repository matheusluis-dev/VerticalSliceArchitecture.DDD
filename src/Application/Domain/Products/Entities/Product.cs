namespace Application.Domain.Products.Entities;

using Application.Domain.__Common.Entities;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Products.Specifications;
using Application.Domain.Products.ValueObjects;

public sealed class Product : EntityBase
{
    public required ProductId Id { get; init; }
    public required InventoryId? InventoryId { get; init; }

    private ProductName _name;
    public required ProductName Name
    {
        get => _name;
        init => _name = value;
    }

    public static Result<Product> Create(ProductName name)
    {
        var product = new Product()
        {
            Id = ProductId.Create(),
            InventoryId = null,
            Name = name,
        };

        var spec = new ProductNameMustNotBeEmptySpecification();
        if (!spec.IsSatisfiedBy(product))
            return Result<Product>.Invalid(new ValidationError("Product name must be defined"));

        return product;
    }

    public Result<Product> UpdateName(ProductName name)
    {
        if (name == Name)
            return Result<Product>.Invalid(new ValidationError("Can not update name to same name"));

        _name = name;

        return this;
    }
}
