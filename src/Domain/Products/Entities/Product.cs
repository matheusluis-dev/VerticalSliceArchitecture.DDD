using Domain.Inventories.Aggregate;
using Domain.Products.Errors;
using Domain.Products.ValueObjects;

namespace Domain.Products.Entities;

public sealed class Product : EntityBase
{
    public required ProductId Id { get; init; }
    public required Inventory? Inventory { get; init; }
    public required ProductName Name { get; init; }

    public bool HasInventory => Inventory is not null;

    private Product(IList<IDomainEvent>? domainEvents = null)
        : base(domainEvents ?? []) { }

    public static Result<Product> Create(ProductName name, ProductId? id = null, Inventory? inventory = null)
    {
        return new Product
        {
            Id = id ?? new ProductId(GuidV7.NewGuid()),
            Inventory = inventory,
            Name = name,
        };
    }

    public Result<Product> UpdateName(ProductName name)
    {
        if (name == Name)
            return Result.Failure(ProductError.Prd001CanNotUpdateNameToTheSameName);

        return new Product
        {
            Id = Id,
            Name = name,
            Inventory = Inventory,
        };
    }
}
