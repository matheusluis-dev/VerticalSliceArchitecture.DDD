namespace Domain.Products.Entities;

using Domain.Common.Entities;
using Domain.Inventories.Aggregate;
using Domain.Products.Specifications;
using Domain.Products.ValueObjects;

public sealed class Product : EntityBase
{
    public required ProductId Id { get; init; }
    public required Inventory? Inventory { get; init; }

    private ProductName _name;
    public required ProductName Name
    {
        get => _name;
        init => _name = value;
    }

    public Product()
        : base([]) { }

    public static Result<Product> Create(ProductName name)
    {
        var product = new Product()
        {
            Id = ProductId.Create(),
            Inventory = null,
            Name = name,
        };

        if (!new ProductNameMustNotBeEmptySpecification().IsSatisfiedBy(product))
            return Result<Product>.Invalid(new ValidationError("Product name must be defined"));

        return product;
    }

    public Result<Product> UpdateName(ProductName name)
    {
        if (name == Name)
            return Result<Product>.Invalid(
                new ValidationError("Can not update name to the same name")
            );

        _name = name;

        return this;
    }
}
