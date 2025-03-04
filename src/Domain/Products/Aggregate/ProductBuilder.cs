using System.Collections.Immutable;
using Domain.Products.Entities;
using Domain.Products.ValueObjects;

namespace Domain.Products.Aggregate;

public sealed class ProductBuilder : IProductBuilder
{
    private Product? _productToClone;

    private ProductId? _id;
    private Quantity? _quantity;
    private Inventory? _inventory;
    private ProductName? _name;
    private IImmutableList<Adjustment> _adjustmentsToAdd = [];
    private IImmutableList<Reservation> _reservationsToAdd = [];
    private IImmutableList<ReservationId> _reservationsToRemove = [];

    private bool _createNewInventory;

    private ProductBuilder() { }

    public static IProductBuilderStart Start()
    {
        return new ProductBuilder();
    }

    public Result<Product> Build()
    {
        return _productToClone is null ? New() : Clone();

        Result<Product> New()
        {
            return Product.Create(
                _id ?? new ProductId(Guid.NewGuid()),
                _inventory,
                _name ?? throw new ArgumentNullException(nameof(_name)),
                domainEvents: null
            );
        }

        Result<Product> Clone()
        {
            var adjustments = _productToClone.GetAdjustments().AddRange(_adjustmentsToAdd);

            var reservations = _productToClone
                .GetReservations()
                .AddRange(_reservationsToAdd.Where(r => !_reservationsToRemove.Contains(r.Id)));

            Inventory? inventory = null;
            if (_productToClone.HasInventory)
            {
                inventory = Inventory.Create(
                    _productToClone.InventoryId!,
                    _productToClone.Id,
                    _quantity
                        ?? _productToClone.Inventory?.Quantity
                        ?? throw new ArgumentNullException(nameof(_quantity)),
                    adjustments,
                    reservations
                );
            }
            else if (_createNewInventory)
            {
                inventory = Inventory.Create(
                    new InventoryId(Guid.NewGuid()),
                    _productToClone.Id,
                    _quantity ?? throw new ArgumentNullException(nameof(_quantity)),
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
        _id = new ProductId(Guid.NewGuid());
        return this;
    }

    public IProductBuilderWithSequence WithId(ProductId id)
    {
        _id = id;
        return this;
    }

    public IProductBuilderWithSequence WithName(ProductName name)
    {
        _name = name;
        return this;
    }

    public IProductBuilderWithSequence WithInventory(Inventory inventory)
    {
        _inventory = inventory;
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

    public IProductBuilderWithPropertiesCloning WithNewInventory()
    {
        _createNewInventory = true;
        return this;
    }

    public IProductBuilderWithPropertiesCloning RemoveReservation(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        _reservationsToRemove = _reservationsToRemove.Add(reservation.Id);
        return this;
    }
}

public interface IProductBuilder : IProductBuilderStart, IProductBuilderWithPropertiesCloning;

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
    IProductBuilderWithSequence WithName(ProductName name);
    IProductBuilderWithSequence WithInventory(Inventory inventory);
    IProductBuilderWithSequence WithQuantity(Quantity quantity);
    IProductBuilderWithSequence WithAdjustment(Adjustment adjustment);
    IProductBuilderWithSequence WithAdjustments(IEnumerable<Adjustment> adjustments);
    IProductBuilderWithSequence WithReservation(Reservation reservation);
    IProductBuilderWithSequence WithReservations(IEnumerable<Reservation> reservations);
}

public interface IProductBuilderWithPropertiesCloning : IProductBuilderWithSequence
{
    IProductBuilderWithPropertiesCloning WithNewInventory();
    IProductBuilderWithPropertiesCloning RemoveReservation(Reservation reservation);
}

public interface IProductBuilderWithSequence : IProductBuilderWithProperties, IProductBuilderBuild;

public interface IProductBuilderBuild
{
    Result<Product> Build();
}
