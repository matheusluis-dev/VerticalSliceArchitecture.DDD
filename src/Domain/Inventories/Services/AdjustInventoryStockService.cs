namespace Domain.Inventories.Services;

using Domain.Common.ValueObjects;
using Domain.Inventories.Aggregate;
using Domain.Inventories.Entities;
using Domain.Inventories.Specifications;
using Domain.Inventories.ValueObjects;

public sealed class AdjustInventoryStockService
{
    public Result<Inventory> DecreaseQuantity(Inventory inventory, Quantity quantity, string reason)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(reason);

        var errors = new List<ValidationError>();

        if (quantity.Value <= 0)
            errors.Add(new ValidationError("Quantity must be greater than 0"));

        if (reason.Length <= 14)
            errors.Add(new ValidationError("Reason must have at least 15 characters"));

        if (!new HasEnoughStockToDecreaseSpecification(quantity).IsSatisfiedBy(inventory))
        {
            errors.Add(
                new ValidationError(
                    $"Quantity to decrease is highest than available stock ({inventory.GetAvailableStock()})"
                )
            );
        }

        if (errors.Count > 0)
            return Result<Inventory>.Invalid(errors);

        var adjustment = new Adjustment
        {
            Id = AdjustmentId.Create(),
            InventoryId = inventory.Id,
            OrderId = null,
            Quantity = Quantity.From(quantity.Value * -1),
            Reason = reason,
        };

        inventory.AddAdjustment(adjustment);

        return inventory;
    }

    public Result<Inventory> IncreaseQuantity(Inventory inventory, Quantity quantity, string reason)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(reason);

        var errors = new List<ValidationError>();

        if (quantity.Value <= 0)
            errors.Add(new ValidationError("Quantity must be greater than 0"));

        if (reason.Length <= 14)
            errors.Add(new ValidationError("Reason must have at least 15 characters"));

        if (errors.Count > 0)
            return Result<Inventory>.Invalid(errors);

        var adjustment = new Adjustment
        {
            Id = AdjustmentId.Create(),
            InventoryId = inventory.Id,
            OrderId = null,
            Quantity = quantity,
            Reason = reason,
        };

        inventory.AddAdjustment(adjustment);

        return inventory;
    }
}
