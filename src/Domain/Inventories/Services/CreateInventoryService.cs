namespace Domain.Inventories.Services;

using Domain.Common.ValueObjects;
using Domain.Inventories.Aggregate;
using Domain.Inventories.Ids;
using Domain.Products.Entities;

public sealed class CreateInventoryService
{
    public Result<Inventory> CreateForProduct(Product product, Quantity quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity is null)
            return Result<Inventory>.Invalid(new ValidationError("Initial quantity must be greater than 0."));

        if (product.HasInventory)
        {
            return Result<Inventory>.Invalid(
                new ValidationError($"Product already has an inventory ({product.Inventory!.Id})")
            );
        }

        return Inventory.Create(new InventoryId(Guid.NewGuid()), product.Id, quantity, [], []);
    }
}
