namespace Application.Domain.Inventories.Aggregate;

using Application.Domain.__Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.Entities;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;
using Application.Domain.Products.ValueObjects;

public sealed class Inventory : IAggregate
{
    public InventoryId Id { get; init; }

    public ProductId ProductId { get; init; }

    public Quantity Quantity { get; init; }

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

    public Quantity GetAvailableStock()
    {
        return Quantity.From(Quantity.Value - GetReservedStock().Value);
    }

    public Quantity GetReservedStock()
    {
        var sum = Reservations.Sum(reservation => reservation.Quantity.Value);

        return Quantity.From(sum);
    }

    public void ReserveStock(OrderItemId orderItemId, Quantity quantity)
    {
        if (quantity.Value <= 0)
            throw new Exception("TODO");

        if (quantity.Value > GetAvailableStock().Value)
            throw new Exception("TODO");

        _reservations.Add(
            new()
            {
                Id = ReservationId.Create(),
                InventoryId = Id,
                OrderItemId = orderItemId,
                Quantity = quantity,
            }
        );
    }

    public void ReleaseStock(OrderItemId orderItemId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.OrderItemId == orderItemId);

        if (reservation is null)
            return;

        _reservations.Remove(reservation);
    }
}
