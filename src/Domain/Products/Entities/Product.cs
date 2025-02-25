namespace Domain.Products.Entities;

using Domain.Common.DomainEvents;
using Domain.Common.Entities;
using Domain.Inventories.Aggregate;
using Domain.Products.Ids;
using Domain.Products.ValueObjects;

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
        ArgumentNullException.ThrowIfNull(name);

        if (!name.IsFilled())
            return Result.Invalid(new ValidationError("Can not create product with empty name."));

        return new Product
        {
            Id = id ?? new ProductId(Guid.NewGuid()),
            Inventory = inventory,
            Name = name,
        };
    }

    public Result<Product> UpdateName(ProductName name)
    {
        if (name == Name)
            return Result.Invalid(new ValidationError("Can not update name to the same name"));

        return new Product
        {
            Id = Id,
            Name = name,
            Inventory = Inventory,
        };
    }
}
