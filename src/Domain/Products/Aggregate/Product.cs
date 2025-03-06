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
    public required InventoryId? InventoryId { get; init; }

    public Inventory? Inventory { get; private init; }

    public bool HasInventory => Inventory is not null;
    public bool CanBeDeleted => !HasInventory || (GetAdjustments().Count is 0 && GetReservations().Count is 0);

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
            Name = name,

            InventoryId = inventory?.Id,
            Inventory = inventory,
        };
    }

    public Result<Product> UpdateName(ProductName name)
    {
        if (name == Name)
            return Result.Failure(ProductError.Prd001CanNotUpdateNameToTheSameName);

        return ProductBuilder.Start().WithProductToClone(this).WithName(name).Build();
    }

    public Result<Product> CreateInventory(Quantity quantity)
    {
        if (HasInventory)
            return Result.Failure(ProductError.Prd003AlreadyHaveInventory);

        return ProductBuilder.Start().WithProductToClone(this).WithNewInventory().WithQuantity(quantity).Build();
    }

    internal IImmutableList<Adjustment> GetAdjustments()
    {
        return Inventory?.Adjustments ?? ImmutableList<Adjustment>.Empty;
    }

    internal IImmutableList<Reservation> GetReservations()
    {
        return Inventory?.Reservations ?? ImmutableList<Reservation>.Empty;
    }

    public Result<Quantity> GetAvailableStock()
    {
        if (!HasInventory)
            return Result.Failure(ProductError.Prd002DoesNotHaveInventory);

        var getReservedStock = GetReservedStock();
        if (getReservedStock.Failed)
            return getReservedStock;

        return new Quantity(Inventory!.Quantity.Value - getReservedStock.Object!.Value);
    }

    private Result<Quantity> GetReservedStock()
    {
        if (!HasInventory)
            return Result.Failure(ProductError.Prd002DoesNotHaveInventory);

        var sum = Inventory!.Reservations.Sum(reservation => reservation.Quantity.Value);

        return new Quantity(sum);
    }

    internal bool HasEnoughStockToDecrease(Quantity quantity)
    {
        if (!HasInventory)
            return true;

        return Inventory!.Quantity.Value >= quantity.Value;
    }

    public Result<Product> IncreaseStock(Quantity quantity, string reason)
    {
        if (!HasInventory)
            return Result.Failure(ProductError.Prd002DoesNotHaveInventory);

        ArgumentNullException.ThrowIfNull(quantity);
        ArgumentNullException.ThrowIfNull(reason);

        var adjustment = Adjustment.Create(new AdjustmentId(Guid.NewGuid()), Inventory!.Id, null, quantity, reason);

        return adjustment.Failed ? Result.Failure(adjustment.Errors) : PlaceAdjustment(adjustment.Object!);
    }

    public Result<Product> DecreaseStock(Quantity quantity, string reason)
    {
        if (!HasInventory)
            return Result.Failure(ProductError.Prd002DoesNotHaveInventory);

        ArgumentNullException.ThrowIfNull(quantity);
        ArgumentNullException.ThrowIfNull(reason);

        var normalizedQuantity = new Quantity(quantity.Value < 0 ? quantity.Value * -1 : quantity.Value);

        if (!HasEnoughStockToDecrease(normalizedQuantity))
            return Result.Failure(InventoryError.Inv003QuantityToDecreaseIsGreaterThanAvailableStock(this));

        var adjustment = Adjustment.Create(
            new AdjustmentId(Guid.NewGuid()),
            Inventory!.Id,
            null,
            new Quantity(normalizedQuantity.Value * -1),
            reason
        );

        return adjustment.Failed ? Result.Failure(adjustment.Errors) : PlaceAdjustment(adjustment.Object!);
    }

    internal Result<Product> PlaceAdjustment(Adjustment newAdjustment)
    {
        if (!HasInventory)
            return Result.Failure(ProductError.Prd002DoesNotHaveInventory);

        ArgumentNullException.ThrowIfNull(newAdjustment);

        var quantity = new Quantity(Inventory!.Quantity.Value + newAdjustment.Quantity.Value);

        var productResult = ProductBuilder
            .Start()
            .WithProductToClone(this)
            .WithAdjustment(newAdjustment)
            .WithQuantity(quantity)
            .Build();

        if (productResult.Failed)
            return Result.Failure(productResult.Errors);

        var product = productResult.Object!;
        if (product.Inventory!.Quantity.Value is 0)
            product.RaiseDomainEvent(new InventoryStockReachedZeroEvent(product));

        return product;
    }

    internal Result<Product> PlaceReservation(Reservation newReservation)
    {
        if (!HasInventory)
            return Result.Failure(ProductError.Prd002DoesNotHaveInventory);

        ArgumentNullException.ThrowIfNull(newReservation);

        return ProductBuilder.Start().WithProductToClone(this).WithReservation(newReservation).Build();
    }

    internal Result<Product> AlterReservationStatus(ReservationId id, ReservationStatus status)
    {
        if (!HasInventory)
            return Result.Failure(ProductError.Prd002DoesNotHaveInventory);

        var oldReservation = Inventory!.Reservations.FirstOrDefault(r => r.Id == id);

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
