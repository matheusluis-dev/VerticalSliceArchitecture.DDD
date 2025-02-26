using Domain.Inventories.Aggregate;
using Domain.Products.Entities;

namespace Domain.Inventories.Services;

public sealed class CreateInventoryService
{
    public Result<Inventory> CreateForProduct(Product product, Quantity quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(quantity);

        if (product.HasInventory)
        {
            return Result<Inventory>.Invalid(
                new ValidationError($"Product already has an inventory ({product.Inventory!.Id})")
            )!;
        }

        return Inventory.Create(new InventoryId(Guid.NewGuid()), product.Id, quantity, [], []);
    }
}
