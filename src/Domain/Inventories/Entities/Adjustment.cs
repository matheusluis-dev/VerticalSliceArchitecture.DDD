using Domain.Inventories.Errors;

namespace Domain.Inventories.Entities;

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
        var errors = new List<Error>();

        if (inventoryId is null)
            errors.Add(AdjustmentError.Adj001InventoryIdMustBeInformed);

        if (string.IsNullOrWhiteSpace(reason))
            errors.Add(AdjustmentError.Adj002ReasonMustBeInformed);
        else if (reason.Length < 15)
            errors.Add(AdjustmentError.Adj003ReasonMustHaveAtLeast15Characters);

        if (errors.Count > 0)
            return Result.Failure(errors);

        return new Adjustment
        {
            Id = id,
            InventoryId = inventoryId!,
            OrderItemId = orderItemId,
            Quantity = quantity,
            Reason = reason,
        };
    }
}
