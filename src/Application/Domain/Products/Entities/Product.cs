namespace Application.Domain.Products.Entities;

using Application.Domain.Common.Entities;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Products.ValueObjects;
using Ardalis.Result;

public sealed class Product : IEntity
{
    public ProductId Id { get; init; }
    public InventoryId? InventoryId { get; init; }
    public ProductName Name { get; init; }

    public static Result<Product> Create(ProductName name)
    {
        if (name.IsNullOrWhiteSpace())
            return Result<Product>.Invalid(new ValidationError("Product name must be defined."));

        return Result.Success<Product>(new() { Id = ProductId.Create(), Name = name });
    }
}
