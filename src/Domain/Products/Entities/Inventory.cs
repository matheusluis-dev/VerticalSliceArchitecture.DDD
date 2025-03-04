using System.Collections.Immutable;

namespace Domain.Products.Entities;

public sealed class Inventory : IChildEntity
{
    public required InventoryId Id { get; init; }
    public required ProductId ProductId { get; init; }
    public required Quantity Quantity { get; init; }
    public required IImmutableList<Adjustment> Adjustments { get; init; }
    public required IImmutableList<Reservation> Reservations { get; init; }

    public bool HasAdjustments => Adjustments.Count > 0;
    public bool HasReservations => Reservations.Count > 0;

    private Inventory() { }

    public static Result<Inventory> Create(
        InventoryId id,
        ProductId productId,
        Quantity quantity,
        IImmutableList<Adjustment> adjustments,
        IImmutableList<Reservation> reservations
    )
    {
        return new Inventory
        {
            Id = id,
            ProductId = productId,
            Quantity = quantity,
            Adjustments = adjustments,
            Reservations = reservations,
        };
    }
}
