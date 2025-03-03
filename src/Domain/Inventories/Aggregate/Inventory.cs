using System.Collections.Immutable;
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
    public required IImmutableList<Adjustment> Adjustments { get; init; }
    public required IImmutableList<Reservation> Reservations { get; init; }

    private Inventory(IList<IDomainEvent>? domainEvents = null)
        : base(domainEvents ?? []) { }

    internal static Result<Inventory> Create(
        InventoryId id,
        ProductId? productId,
        Quantity quantity,
        IImmutableList<Adjustment> adjustments,
        IImmutableList<Reservation> reservations,
        IImmutableList<IDomainEvent>? domainEvents
    )
    {
        if (productId is null)
            return Result.Failure(InventoryError.Inv002ProductIdMustBeInformed);

        return new Inventory(domainEvents?.ToList())
        {
            Id = id,
            ProductId = productId,
            Quantity = quantity,
            Adjustments = adjustments,
            Reservations = reservations,
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

        var adjustment = Adjustment.Create(new AdjustmentId(GuidV7.NewGuid()), Id, null, quantity, reason);

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
            new AdjustmentId(GuidV7.NewGuid()),
            Id,
            null,
            new Quantity(normalizedQuantity.Value * -1),
            reason
        );

        return adjustment.Failed ? Result.Failure(adjustment.Errors) : PlaceAdjustment(adjustment.Value!);
    }

    internal Result<Inventory> PlaceAdjustment(Adjustment newAdjustment)
    {
        ArgumentNullException.ThrowIfNull(newAdjustment);

        var quantity = new Quantity(Quantity.Value + newAdjustment.Quantity.Value);

        var inventoryResult = InventoryBuilder
            .Start()
            .WithInventoryToClone(this)
            .WithAdjustment(newAdjustment)
            .WithQuantity(quantity)
            .Build();

        if (inventoryResult.Failed)
            return Result.Failure(inventoryResult.Errors);

        var inventory = inventoryResult.Value!;
        if (inventory.Quantity.Value is 0)
            inventory.RaiseDomainEvent(new InventoryStockReachedZeroEvent(inventory, ProductId));

        return inventory;
    }

    internal Result<Inventory> PlaceReservation(Reservation newReservation)
    {
        ArgumentNullException.ThrowIfNull(newReservation);

        return InventoryBuilder.Start().WithInventoryToClone(this).WithReservation(newReservation).Build();
    }

    internal Result<Inventory> AlterReservationStatus(ReservationId id, ReservationStatus status)
    {
        var oldReservation = Reservations.FirstOrDefault(r => r.Id == id);

        if (oldReservation is null)
            return Result.Failure(InventoryError.Inv004ReservationWithIdNotFound(id));

        var newReservation = Reservation.Create(
            oldReservation.Id,
            oldReservation.InventoryId,
            oldReservation.OrderItemId,
            oldReservation.Quantity,
            status
        );

        return newReservation.Failed
            ? Result.Failure(newReservation.Errors)
            : InventoryBuilder
                .Start()
                .WithInventoryToClone(this)
                .RemoveReservation(oldReservation)
                .WithReservation(newReservation)
                .Build();
    }
}
