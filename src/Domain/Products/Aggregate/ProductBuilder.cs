using System.Collections.Immutable;
using Domain.Products.Entities;
using Domain.Products.ValueObjects;

namespace Domain.Products.Aggregate;

public sealed class ProductBuilder : IProductBuilder
{
    private Product? _productToClone;

    private ProductId? _id;
    private Quantity? _quantity;
    private ProductName? _name;
    private Inventory? _inventory;
    private IImmutableList<Adjustment> _adjustmentsToAdd = [];
    private IImmutableList<Reservation> _reservationsToAdd = [];
    private IImmutableList<ReservationId> _reservationsToRemove = [];

    private ProductBuilder() { }

    public static IProductBuilderStart Start()
    {
        return new ProductBuilder();
    }

    public Result<Product> Build()
    {
        ArgumentNullException.ThrowIfNull(_name);
        ArgumentNullException.ThrowIfNull(_quantity);

        return _productToClone is null ? New() : Clone();

        Result<Product> New()
        {
            return Product.Create(_id ?? new ProductId(GuidV7.NewGuid()), _inventory, _name, domainEvents: null);
        }

        Result<Product> Clone()
        {
            Inventory? inventory = null;
            if (_productToClone.HasInventory)
            {
                var adjustments = _productToClone.Inventory!.Adjustments.AddRange(_adjustmentsToAdd);

                var reservations = _productToClone.Inventory.Reservations.AddRange(
                    _reservationsToAdd.Where(r => !_reservationsToRemove.Contains(r.Id))
                );

                inventory = Inventory.Create(
                    _productToClone.Inventory.Id,
                    _productToClone.Id,
                    _quantity,
                    adjustments,
                    reservations
                );
            }

            return Product.Create(
                _productToClone!.Id,
                inventory,
                _name ?? _productToClone.Name,
                [.. _productToClone.GetDomainEvents()]
            );
        }
    }

    public IProductBuilderWithPropertiesCloning WithProductToClone(Product product)
    {
        _productToClone = product;
        return this;
    }

    public IProductBuilderWithSequence WithNewId()
    {
        _id = new ProductId(GuidV7.NewGuid());
        return this;
    }

    public IProductBuilderWithSequence WithId(ProductId id)
    {
        _id = id;
        return this;
    }

    public IProductBuilderWithSequence WithQuantity(Quantity quantity)
    {
        _quantity = quantity;
        return this;
    }

    public IProductBuilderWithSequence WithAdjustment(Adjustment adjustment)
    {
        _adjustmentsToAdd = _adjustmentsToAdd.Add(adjustment);
        return this;
    }

    public IProductBuilderWithSequence WithAdjustments(IEnumerable<Adjustment> adjustments)
    {
        _adjustmentsToAdd = _adjustmentsToAdd.AddRange(adjustments);
        return this;
    }

    public IProductBuilderWithSequence WithReservation(Reservation reservation)
    {
        _reservationsToAdd = _reservationsToAdd.Add(reservation);
        return this;
    }

    public IProductBuilderWithSequence WithReservations(IEnumerable<Reservation> reservations)
    {
        _reservationsToAdd = _reservationsToAdd.AddRange(reservations);
        return this;
    }

    public IProductBuilderWithPropertiesCloning RemoveReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        _reservationsToRemove = _reservationsToRemove.Add(reservation.Id);
        return this;
    }
}

public interface IProductBuilder
    : IProductBuilderStart,
        IProductBuilderWithSequence,
        IProductBuilderWithPropertiesCloning;

public interface IProductBuilderStart : IProductBuilderClone, IProductBuilderWithId;

public interface IProductBuilderClone
{
    IProductBuilderWithPropertiesCloning WithProductToClone(Product product);
}

public interface IProductBuilderWithId
{
    IProductBuilderWithSequence WithNewId();
    IProductBuilderWithSequence WithId(ProductId id);
}

public interface IProductBuilderWithProperties
{
    IProductBuilderWithSequence WithQuantity(Quantity quantity);
    IProductBuilderWithSequence WithAdjustment(Adjustment adjustment);
    IProductBuilderWithSequence WithAdjustments(IEnumerable<Adjustment> adjustments);
    IProductBuilderWithSequence WithReservation(Reservation reservation);
    IProductBuilderWithSequence WithReservations(IEnumerable<Reservation> reservations);
}

public interface IProductBuilderWithPropertiesCloning : IProductBuilderWithProperties
{
    IProductBuilderWithPropertiesCloning RemoveReservation(Reservation reservation);
}

public interface IProductBuilderWithSequence : IProductBuilderWithProperties, IProductBuilderBuild;

public interface IProductBuilderBuild
{
    Result<Product> Build();
}
