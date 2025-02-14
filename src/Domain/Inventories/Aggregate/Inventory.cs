namespace Domain.Inventories.Aggregate;

using Domain.Common.Entities;
using Domain.Common.ValueObjects;
using Domain.Inventories.Entities;
using Domain.Inventories.Events;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;
using Domain.Products.ValueObjects;

public sealed class Inventory : EntityBase
{
    public InventoryId Id { get; init; }

    public ProductId ProductId { get; init; }

    public Quantity Quantity { get; set; }

    private readonly List<Adjustment> _adjustments;
    public IReadOnlyCollection<Adjustment> Adjustments => _adjustments.AsReadOnly();

    private readonly List<Reservation> _reservations;
    public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

    public Inventory(
        InventoryId id,
        ProductId productId,
        Quantity quantity,
        IEnumerable<Adjustment> adjustments,
        IEnumerable<Reservation> reservations
    )
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        _adjustments = [.. adjustments];
        _reservations = [.. reservations];
    }

    public static Result<Inventory> CreateForProduct(ProductId productId, Quantity quantity)
    {
        if (quantity.Value <= 0)
        {
            return Result<Inventory>.Invalid(
                new ValidationError("Initial quantity must be greater than 0.")
            );
        }

        return new Inventory(InventoryId.Create(), productId, quantity, [], []);
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

    internal void AddAdjustment(Adjustment adjustment)
    {
        ArgumentNullException.ThrowIfNull(adjustment);

        _adjustments.Add(adjustment);
        Quantity = Quantity.From(Quantity.Value + adjustment.Quantity.Value);

        if (Quantity.Value == 0)
            RaiseDomainEvent(new InventoryStockReachedZeroEvent(Id));
    }

    public Result<Reservation> ReserveStock(OrderItemId orderItemId, Quantity quantity)
    {
        if (quantity.Value <= 0)
        {
            return Result<Reservation>.Invalid(
                new ValidationError("Quantity must be greater than 0")
            );
        }

        // TODO: use specification
        var availableStock = GetAvailableStock();
        if (quantity.Value > availableStock.Value)
        {
            return Result<Reservation>.Invalid(
                new ValidationError(
                    $"Reservation quantity ({quantity}) is greater than the available stock ({availableStock})"
                )
            );
        }

        var reservation = new Reservation
        {
            Id = ReservationId.Create(),
            InventoryId = Id,
            OrderItemId = orderItemId,
            Quantity = quantity,
        };

        _reservations.Add(reservation);

        return reservation;
    }

    public void ReleaseStock(OrderItemId orderItemId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.OrderItemId == orderItemId);

        if (reservation is null)
            return;

        _reservations.Remove(reservation);
    }
}
