using Domain.Inventories.Entities;
using Domain.Inventories.Enums;
using Domain.Inventories.Errors;
using Domain.Inventories.Events;

namespace Domain.Inventories.Aggregate;

public sealed class Inventory : EntityBase
{
    public required InventoryId Id { get; init; }

    public required ProductId ProductId { get; init; }

    public required Quantity Quantity { get; init; }

    public required IReadOnlyCollection<Adjustment> Adjustments { get; init; }

    public required IReadOnlyCollection<Reservation> Reservations { get; init; }

    private Inventory(IList<IDomainEvent>? domainEvents = null)
        : base(domainEvents ?? []) { }

    public static Result<Inventory> Create(
        InventoryId id,
        ProductId? productId,
        Quantity? quantity,
        IEnumerable<Adjustment> adjustments,
        IEnumerable<Reservation> reservations,
        IList<IDomainEvent>? domainEvents = null
    )
    {
        if (productId is null)
            return Result.Failure(InventoryError.Inv002ProductIdMustBeInformed);

        return new Inventory(domainEvents)
        {
            Id = id,
            ProductId = productId!,
            Quantity = quantity!,
            Adjustments = adjustments.ToList().AsReadOnly(),
            Reservations = reservations.ToList().AsReadOnly(),
        };
    }

    public Quantity GetAvailableStock()
    {
        return new Quantity(Quantity.Value - GetReservedStock().Value);
    }

    private Quantity GetReservedStock()
    {
        var sum = Reservations.Sum(reservation => reservation.Quantity.Value);

        return new Quantity(sum);
    }

    internal bool HasEnoughStockToDecrease(Quantity quantity)
    {
        return Quantity.Value >= quantity.Value;
    }

    public Result<Inventory> IncreaseStock(Quantity quantity, string reason)
    {
        ArgumentNullException.ThrowIfNull(quantity);
        ArgumentNullException.ThrowIfNull(reason);

        var adjustment = Adjustment.Create(new AdjustmentId(Guid.NewGuid()), Id, null, quantity, reason);

        return adjustment.Failed ? Result.Failure(adjustment.Errors) : PlaceAdjustment(adjustment.Value!);
    }

    public Result<Inventory> DecreaseStock(Quantity quantity, string reason)
    {
        ArgumentNullException.ThrowIfNull(quantity);
        ArgumentNullException.ThrowIfNull(reason);

        var normalizedQuantity = new Quantity(quantity.Value < 0 ? quantity.Value * -1 : quantity.Value);

        if (!HasEnoughStockToDecrease(normalizedQuantity))
            return Result.Failure(InventoryError.Inv003QuantityToDecreaseIsGreaterThanAvailableStock(this));

        var adjustment = Adjustment.Create(
            new AdjustmentId(Guid.NewGuid()),
            Id,
            null,
            new Quantity(normalizedQuantity.Value * -1),
            reason
        );

        return adjustment.Failed ? Result.Failure(adjustment.Errors) : PlaceAdjustment(adjustment.Value!);
    }

    internal Result<Inventory> PlaceAdjustment(Adjustment adjustment)
    {
        ArgumentNullException.ThrowIfNull(adjustment);

        var quantity = new Quantity(Quantity.Value + adjustment.Quantity.Value);

        var inventoryResult = new InventoryBuilder()
            .WithInventoryToClone(this)
            .WithAdjustment(Adjustments)
            .WithAdjustment(adjustment)
            .WithQuantity(quantity)
            .Build();

        if (inventoryResult.Failed)
            return Result.Failure(inventoryResult.Errors);

        var inventory = inventoryResult.Value!;
        if (inventory.Quantity.Value is 0)
            inventory.RaiseDomainEvent(new InventoryStockReachedZeroEvent(this, ProductId));

        return inventory;
    }

    internal Result<Inventory> PlaceReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        return new InventoryBuilder()
            .WithInventoryToClone(this)
            .WithReservations(Reservations)
            .WithReservations(reservation)
            .Build();
    }

    internal Result<Inventory> AlterReservationStatus(ReservationId id, ReservationStatus status)
    {
        var reservation = Reservations.FirstOrDefault(r => r.Id == id);

        if (reservation is null)
            return Result.Failure(InventoryError.Inv004ReservationWithIdNotFound(id));

        var newReservation = Reservation.Create(
            reservation.Id,
            reservation.InventoryId,
            reservation.OrderItemId,
            reservation.Quantity,
            status
        );

        return newReservation.Failed
            ? Result.Failure(newReservation.Errors)
            : new InventoryBuilder()
                .WithInventoryToClone(this)
                .WithReservations([.. Reservations.Except([reservation]), newReservation.Value!])
                .Build();
    }
}
