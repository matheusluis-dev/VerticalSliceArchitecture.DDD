namespace Domain.Inventories.Aggregate;

using Domain.Common.DomainEvents;
using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.Entities;
using Domain.Inventories.Events;
using Domain.Inventories.ValueObjects;
using Domain.Products.ValueObjects;

public sealed class Inventory : EntityBase
{
    public InventoryId Id { get; init; }

    public ProductId ProductId { get; init; }

    public Quantity Quantity { get; init; }

    public IReadOnlyCollection<Adjustment> Adjustments { get; init; }

    public IReadOnlyCollection<Reservation> Reservations { get; init; }

    private Inventory(
        InventoryId id,
        ProductId productId,
        Quantity quantity,
        IEnumerable<Adjustment> adjustments,
        IEnumerable<Reservation> reservations,
        IList<IDomainEvent>? domainEvents = null
    )
        : base(domainEvents ?? [])
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        Adjustments = adjustments.ToList().AsReadOnly();
        Reservations = reservations.ToList().AsReadOnly();
    }

    public static Result<Inventory> Create(
        InventoryId id,
        ProductId? productId,
        Quantity quantity,
        IEnumerable<Adjustment> adjustments,
        IEnumerable<Reservation> reservations,
        IList<IDomainEvent>? domainEvents = null
    )
    {
        if (productId is null)
            return Result.Invalid(new ValidationError($"{nameof(ProductId)} must be set"));

        return new Inventory(id, productId.Value, quantity, adjustments, reservations, domainEvents);
    }

    public Quantity GetAvailableStock()
    {
        return Quantity.From(Quantity.Value - GetReservedStock().Value);
    }

    public Quantity GetReservedStock()
    {
        var sum = Reservations.Sum(reservation => reservation.Quantity.Value);

        return Quantity.From(sum);
    }

    internal Result<Inventory> PlaceAdjustment(Adjustment adjustment)
    {
        ArgumentNullException.ThrowIfNull(adjustment);

        var quantity = Quantity.From(Quantity.Value + adjustment.Quantity.Value);

        var buildResult = new InventoryBuilder()
            .WithInventoryToClone(this)
            .WithAdjustment(Adjustments)
            .WithAdjustment(adjustment)
            .WithQuantity(quantity)
            .Build();

        if (buildResult.IsInvalid())
            return Result<Inventory>.Invalid(buildResult.ValidationErrors);

        var inventory = buildResult.Value;

        if (inventory.Quantity.Value == 0)
            inventory.RaiseDomainEvent(new InventoryStockReachedZeroEvent(this, ProductId));

        return inventory;
    }

    internal Result<Inventory> PlaceReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        return new InventoryBuilder()
            .WithInventoryToClone(this)
            .WithReservation(Reservations)
            .WithReservation(reservation)
            .Build();
    }
}
