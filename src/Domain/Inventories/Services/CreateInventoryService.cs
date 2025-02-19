namespace Domain.Inventories.Services;

using Domain.Common.ValueObjects;
using Domain.Inventories.Aggregate;
using Domain.Inventories.ValueObjects;
using Domain.Products.Entities;
using Domain.Products.Specifications;

public sealed class CreateInventoryService
{
    public Result<Inventory> CreateForProduct(Product product, Quantity quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity.Value <= 0)
            return Result<Inventory>.Invalid(new ValidationError("Initial quantity must be greater than 0."));

        if (new HasInventorySpecification().IsSatisfiedBy(product))
        {
            return Result<Inventory>.Invalid(
                new ValidationError($"Product already has an inventory ({product.Inventory!.Id})")
            );
        }

        return Inventory.Create(InventoryId.Create(), product.Id, quantity, [], []);
    }
}
