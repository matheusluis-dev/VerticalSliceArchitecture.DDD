using Domain.Products.Enums;
using Domain.Products.Errors;

namespace Domain.Products.Entities;

public sealed class Reservation : IChildEntity
{
    public ReservationId Id { get; init; } = null!;
    public InventoryId InventoryId { get; init; } = null!;
    public OrderItemId OrderItemId { get; init; } = null!;
    public Quantity Quantity { get; init; } = null!;
    public ReservationStatus Status { get; init; }

    private Reservation() { }

    public static Result<Reservation> Create(
        ReservationId id,
        InventoryId? inventoryId,
        OrderItemId? orderItemId,
        Quantity quantity,
        ReservationStatus status
    )
    {
        var errors = new List<Error>();

        if (inventoryId is null)
            errors.Add(ReservationError.Res001InventoryIdMustBeInformed);

        if (orderItemId is null)
            errors.Add(ReservationError.Res002OrderItemIdMustBeInformed);

        if (errors.Count > 0)
            return Result.Failure(errors);

        var reservation = new Reservation
        {
            Id = id,
            InventoryId = inventoryId!,
            OrderItemId = orderItemId!,
            Quantity = quantity!,
            Status = status,
        };

        return reservation;
    }
}
