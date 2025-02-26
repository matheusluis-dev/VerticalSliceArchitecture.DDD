using Domain.Inventories.Enums;

namespace Domain.Inventories.Entities;

public sealed class Reservation : IChildEntity
{
    public ReservationId Id { get; private init; } = null!;
    public InventoryId InventoryId { get; private init; } = null!;
    public OrderItemId OrderItemId { get; private init; } = null!;
    public Quantity Quantity { get; private init; } = null!;
    public ReservationStatus Status { get; private init; }

    private Reservation() { }

    public static Result<Reservation> Create(
        ReservationId id,
        InventoryId? inventoryId,
        OrderItemId? orderItemId,
        Quantity? quantity,
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
