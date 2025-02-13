namespace Application.Domain.Inventories.Services;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.Aggregate;
using Application.Domain.Inventories.Entities;
using Application.Domain.Inventories.ValueObjects;

public sealed class AdjustInventoryStockService
{
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
        inventory.Quantity = Quantity.From(inventory.Quantity.Value + quantity.Value);

        return inventory;
    }
}
