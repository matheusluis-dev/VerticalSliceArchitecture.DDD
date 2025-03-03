using System.Collections.Immutable;
using Domain.Inventories.Entities;

namespace Domain.Inventories.Aggregate;

public sealed class InventoryBuilder : IInventoryBuilder
{
    private Inventory? _inventoryToClone;
    private InventoryId? _id;
    private ProductId? _productId;
    private Quantity? _quantity;
    private IImmutableList<Adjustment> _adjustmentsToAdd = [];
    private IImmutableList<Reservation> _reservationsToAdd = [];
    private IImmutableList<ReservationId> _reservationsToRemove = [];

    private InventoryBuilder() { }

    public static IInventoryBuilderStart Start()
    {
        return new InventoryBuilder();
    }

    public Result<Inventory> Build()
    {
        ArgumentNullException.ThrowIfNull(_quantity);

        return _inventoryToClone is null ? New() : Clone();

        Result<Inventory> New()
        {
            return Inventory.Create(
                _id ?? new InventoryId(GuidV7.NewGuid()),
                _productId,
                _quantity,
                _adjustmentsToAdd,
                _reservationsToAdd,
                domainEvents: null
            );
        }

        Result<Inventory> Clone()
        {
            var adjustments = _inventoryToClone.Adjustments.AddRange(_adjustmentsToAdd);
            var reservations = _inventoryToClone.Reservations.AddRange(
                _reservationsToAdd.Where(r => !_reservationsToRemove.Contains(r.Id))
            );

            return Inventory.Create(
                _inventoryToClone!.Id,
                _productId ?? _inventoryToClone.ProductId,
                _quantity ?? _inventoryToClone.Quantity,
                adjustments,
                reservations,
                [.. _inventoryToClone.GetDomainEvents()]
            );
        }
    }

    public IInventoryBuilderWithPropertiesCloning WithInventoryToClone(Inventory inventoryToClone)
    {
        _inventoryToClone = inventoryToClone;
        return this;
    }

    public IInventoryBuilderWithSequence WithNewId()
    {
        _id = new InventoryId(GuidV7.NewGuid());
        return this;
    }

    public IInventoryBuilderWithSequence WithId(InventoryId id)
    {
        _id = id;
        return this;
    }

    public IInventoryBuilderWithSequence WithProductId(ProductId productId)
    {
        _productId = productId;
        return this;
    }

    public IInventoryBuilderWithSequence WithQuantity(Quantity quantity)
    {
        _quantity = quantity;
        return this;
    }

    public IInventoryBuilderWithSequence WithAdjustment(Adjustment adjustment)
    {
        _adjustmentsToAdd = _adjustmentsToAdd.Add(adjustment);
        return this;
    }

    public IInventoryBuilderWithSequence WithAdjustments(IEnumerable<Adjustment> adjustments)
    {
        _adjustmentsToAdd = _adjustmentsToAdd.AddRange(adjustments);
        return this;
    }

    public IInventoryBuilderWithSequence WithReservation(Reservation reservation)
    {
        _reservationsToAdd = _reservationsToAdd.Add(reservation);
        return this;
    }

    public IInventoryBuilderWithSequence WithReservations(IEnumerable<Reservation> reservations)
    {
        _reservationsToAdd = _reservationsToAdd.AddRange(reservations);
        return this;
    }

    public IInventoryBuilderWithPropertiesCloning RemoveReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        _reservationsToRemove = _reservationsToRemove.Add(reservation.Id);
        return this;
    }
}

public interface IInventoryBuilder
    : IInventoryBuilderStart,
        IInventoryBuilderWithSequence,
        IInventoryBuilderWithPropertiesCloning;

public interface IInventoryBuilderStart : IInventoryBuilderClone, IInventoryBuilderWithId;

public interface IInventoryBuilderClone
{
    IInventoryBuilderWithPropertiesCloning WithInventoryToClone(Inventory inventoryToClone);
}

public interface IInventoryBuilderWithId
{
    IInventoryBuilderWithSequence WithNewId();
    IInventoryBuilderWithSequence WithId(InventoryId id);
}

public interface IInventoryBuilderWithProperties
{
    IInventoryBuilderWithSequence WithProductId(ProductId productId);
    IInventoryBuilderWithSequence WithQuantity(Quantity quantity);
    IInventoryBuilderWithSequence WithAdjustment(Adjustment adjustment);
    IInventoryBuilderWithSequence WithAdjustments(IEnumerable<Adjustment> adjustments);
    IInventoryBuilderWithSequence WithReservation(Reservation reservation);
    IInventoryBuilderWithSequence WithReservations(IEnumerable<Reservation> reservations);
}

public interface IInventoryBuilderWithPropertiesCloning : IInventoryBuilderWithProperties
{
    IInventoryBuilderWithPropertiesCloning RemoveReservation(Reservation reservation);
}

public interface IInventoryBuilderWithSequence : IInventoryBuilderWithProperties, IInventoryBuilderBuild;

public interface IInventoryBuilderBuild
{
    Result<Inventory> Build();
}
