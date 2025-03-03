using System.Collections.Immutable;
using Domain.Products.Entities;
using Domain.Products.Enums;
using Domain.Products.Errors;
using Domain.Products.Events;
using Domain.Products.ValueObjects;

namespace Domain.Products.Aggregate;

public sealed class Product : AggregateBase
{
    public required ProductId Id { get; init; }
    public required ProductName Name { get; init; }

    private readonly Inventory? _inventory;

    public bool HasInventory => _inventory is not null;

    private Product(IImmutableList<IDomainEvent>? domainEvents = null)
        : base(domainEvents ?? []) { }

    internal static Result<Product> Create(
        ProductId id,
        Inventory? inventory,
        ProductName name,
        IImmutableList<IDomainEvent>? domainEvents
    )
    {
        return new Product(domainEvents)
        {
            Id = id,
            _inventory = inventory,
            Name = name,
        };
    }

    public Result<Product> UpdateName(ProductName name)
    {
        if (name == Name)
            return Result.Failure(ProductError.Prd001CanNotUpdateNameToTheSameName);

        return new Product
        {
            Id = Id,
            Name = name,
            _inventory = _inventory,
        };
    }

    public Quantity GetAvailableStock()
    {
        if (!HasInventory)
            throw new Exception("Inventory is null");

        return new Quantity(_inventory!.Quantity.Value - GetReservedStock().Value);
    }

    private Quantity GetReservedStock()
    {
        if (!HasInventory)
            throw new Exception("Inventory is null");

        var sum = _inventory!.Reservations.Sum(reservation => reservation.Quantity.Value);

        return new Quantity(sum);
    }

    internal bool HasEnoughStockToDecrease(Quantity quantity)
    {
        if (!HasInventory)
            return true;

        return _inventory!.Quantity.Value >= quantity.Value;
    }

    public Result<Product> IncreaseStock(Quantity quantity, string reason)
    {
        if (!HasInventory)
            throw new Exception("Inventory is null");

        ArgumentNullException.ThrowIfNull(quantity);
        ArgumentNullException.ThrowIfNull(reason);

        var adjustment = Adjustment.Create(new AdjustmentId(GuidV7.NewGuid()), _inventory!.Id, null, quantity, reason);

        return adjustment.Failed ? Result.Failure(adjustment.Errors) : PlaceAdjustment(adjustment.Value!);
    }

    public Result<Product> DecreaseStock(Quantity quantity, string reason)
    {
        if (!HasInventory)
            throw new Exception("Inventory is null");

        ArgumentNullException.ThrowIfNull(quantity);
        ArgumentNullException.ThrowIfNull(reason);

        var normalizedQuantity = new Quantity(quantity.Value < 0 ? quantity.Value * -1 : quantity.Value);

        if (!HasEnoughStockToDecrease(normalizedQuantity))
            return Result.Failure(InventoryError.Inv003QuantityToDecreaseIsGreaterThanAvailableStock(_inventory!));

        var adjustment = Adjustment.Create(
            new AdjustmentId(GuidV7.NewGuid()),
            _inventory!.Id,
            null,
            new Quantity(normalizedQuantity.Value * -1),
            reason
        );

        return adjustment.Failed ? Result.Failure(adjustment.Errors) : PlaceAdjustment(adjustment.Value!);
    }

    internal Result<Product> PlaceAdjustment(Adjustment newAdjustment)
    {
        if (!HasInventory)
            throw new Exception("Inventory is null");

        ArgumentNullException.ThrowIfNull(newAdjustment);

        var quantity = new Quantity(_inventory!.Quantity.Value + newAdjustment.Quantity.Value);

        var productResult = ProductBuilder
            .Start()
            .WithProductToClone(this)
            .WithAdjustment(newAdjustment)
            .WithQuantity(quantity)
            .Build();

        if (productResult.Failed)
            return Result.Failure(productResult.Errors);

        var product = productResult.Value!;
        if (product._inventory!.Quantity.Value is 0)
            product.RaiseDomainEvent(new InventoryStockReachedZeroEvent(product._inventory, product.Id));

        return product;
    }

    internal Result<Product> PlaceReservation(Reservation newReservation)
    {
        if (!HasInventory)
            throw new Exception("Inventory is null");

        ArgumentNullException.ThrowIfNull(newReservation);

        return ProductBuilder.Start().WithProductToClone(this).WithReservation(newReservation).Build();
    }

    internal Result<Product> AlterReservationStatus(ReservationId id, ReservationStatus status)
    {
        if (!HasInventory)
            throw new Exception("Inventory is null");

        var oldReservation = _inventory!.Reservations.FirstOrDefault(r => r.Id == id);

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
            : ProductBuilder
                .Start()
                .WithProductToClone(this)
                .RemoveReservation(oldReservation)
                .WithReservation(newReservation)
                .Build();
    }
}
