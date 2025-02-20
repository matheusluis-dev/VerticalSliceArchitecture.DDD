namespace Domain.Inventories.Entities;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;

public sealed class Adjustment : IChildEntity
{
    public required AdjustmentId Id { get; init; }
    public required InventoryId InventoryId { get; init; }
    public OrderItemId? OrderItemId { get; init; }
    public required Quantity Quantity { get; init; }
    public required string Reason { get; init; }

    private Adjustment() { }

    public static Result<Adjustment> Create(
        AdjustmentId id,
        InventoryId? inventoryId,
        OrderItemId? orderItemId,
        Quantity quantity,
        string reason
    )
    {
        var errors = new List<ValidationError>();

        if (inventoryId is null)
            errors.Add(new ValidationError($"{nameof(InventoryId)} must be informed"));

        if (string.IsNullOrWhiteSpace(reason))
            errors.Add(new ValidationError("Reason must be informed"));
        else if (reason.Length < 15)
            errors.Add(new ValidationError("Reason must have at least 15 characters"));

        if (quantity.Value == 0)
            errors.Add(new ValidationError("Quantity must be greater than zero"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new Adjustment
        {
            Id = id,
            InventoryId = inventoryId!.Value,
            OrderItemId = orderItemId,
            Quantity = quantity,
            Reason = reason,
        };
    }
}
