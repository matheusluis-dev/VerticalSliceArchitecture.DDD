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
            : InventoryBuilder
                .Start()
                .WithId(new InventoryId(GuidV7.NewGuid()))
                .WithProductId(product.Id)
                .WithQuantity(quantity)
                .Build();
    }
}
