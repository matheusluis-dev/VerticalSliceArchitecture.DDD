using Domain.Inventories.Entities;
using Domain.Inventories.Enums;
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
        var errors = new List<ValidationError>();

        if (productId is null)
            errors.Add(new ValidationError($"{nameof(ProductId)} must be set"));

        if (quantity is null)
            errors.Add(new ValidationError("Initial quantity must be greater than 0."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

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

        if (quantity.Value <= 0)
            return Result.Invalid(new ValidationError("Quantity must be greater than 0"));

        var adjustment = Adjustment.Create(new AdjustmentId(Guid.NewGuid()), Id, null, quantity, reason);

        if (adjustment.IsInvalid())
            return Result.Invalid(adjustment.ValidationErrors!);

        var adjustmentResult = PlaceAdjustment(adjustment);

        if (adjustmentResult.IsInvalid())
            return Result.Invalid(adjustmentResult.ValidationErrors!);

        return adjustmentResult;
    }

    public Result<Inventory> DecreaseStock(Quantity quantity, string reason)
    {
        ArgumentNullException.ThrowIfNull(quantity);
        ArgumentNullException.ThrowIfNull(reason);

        var normalizedQuantity = new Quantity(quantity.Value < 0 ? quantity.Value * -1 : quantity.Value);

        if (!HasEnoughStockToDecrease(normalizedQuantity))
        {
            return Result.Invalid(
                new ValidationError(
                    $"Quantity to decrease is greater than available stock ({GetAvailableStock().Value})"
                )
            );
        }

        var adjustment = Adjustment.Create(
            new AdjustmentId(Guid.NewGuid()),
            Id,
            null,
            new Quantity(normalizedQuantity.Value * -1),
            reason
        );

        if (adjustment.IsInvalid())
            return Result.Invalid(adjustment.ValidationErrors!);

        var adjustmentResult = PlaceAdjustment(adjustment);

        return adjustmentResult.IsInvalid() ? Result.Invalid(adjustmentResult.ValidationErrors!) : adjustmentResult;
    }

    internal Result<Inventory> PlaceAdjustment(Adjustment adjustment)
    {
        ArgumentNullException.ThrowIfNull(adjustment);

        var quantity = new Quantity(Quantity.Value + adjustment.Quantity.Value);

        var buildResult = new InventoryBuilder()
            .WithInventoryToClone(this)
            .WithAdjustment(Adjustments)
            .WithAdjustment(adjustment)
            .WithQuantity(quantity)
            .Build();

        if (buildResult.IsInvalid())
            return Result.Invalid(buildResult.ValidationErrors!);

        var inventory = buildResult.Value!;

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
            return Result.Invalid(new ValidationError($"Reservation '{id}' not found"));

        var newReservation = Reservation.Create(
            reservation.Id,
            reservation.InventoryId,
            reservation.OrderItemId,
            reservation.Quantity,
            status
        );

        if (newReservation.IsInvalid())
            return Result.Invalid(newReservation.ValidationErrors!);

        return new InventoryBuilder()
            .WithInventoryToClone(this)
            .WithReservations([.. Reservations.Except([reservation]), newReservation])
            .Build();
    }
}
