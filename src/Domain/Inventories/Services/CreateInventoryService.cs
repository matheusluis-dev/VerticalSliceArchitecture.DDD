using Domain.Inventories.Aggregate;
using Domain.Inventories.Errors;
using Domain.Products.Entities;

namespace Domain.Inventories.Services;

public sealed class CreateInventoryService
{
    public Result<Inventory> CreateForProduct(Product product, Quantity quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(quantity);

        return product.HasInventory
            ? Result.Failure(InventoryError.Inv001ProductAlreadyHasAnInventory(product.Inventory!.Id))
            : Inventory.Create(new InventoryId(Guid.NewGuid()), product.Id, quantity, [], []);
    }
}
