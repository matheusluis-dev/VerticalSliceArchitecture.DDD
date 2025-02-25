namespace Domain.Inventories.Entities;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.Enums;
using Domain.Inventories.Ids;
using Domain.Orders.Ids;

public sealed class Reservation : IChildEntity
{
    public required ReservationId Id { get; init; }
    public required InventoryId InventoryId { get; init; }
    public required OrderItemId OrderItemId { get; init; }
    public required Quantity Quantity { get; init; }
    public required ReservationStatus Status { get; init; }

    private Reservation() { }

    public static Result<Reservation> Create(
        ReservationId id,
        InventoryId? inventoryId,
        OrderItemId? orderItemId,
        Quantity quantity,
        ReservationStatus status
    )
    {
        var errors = new List<ValidationError>();

        if (inventoryId is null)
            errors.Add(new ValidationError($"{nameof(InventoryId)} must be informed"));

        if (orderItemId is null)
            errors.Add(new ValidationError($"{nameof(OrderItemId)} must be informed"));

        if (quantity is null)
            errors.Add(new ValidationError("Quantity must be greater than zero"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new Reservation
        {
            Id = id,
            InventoryId = inventoryId!,
            OrderItemId = orderItemId!,
            Quantity = quantity!,
            Status = status,
        };
    }
}
