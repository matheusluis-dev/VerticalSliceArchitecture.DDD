using Domain.Inventories.Entities;

namespace Domain.Inventories.Aggregate;

public sealed class InventoryBuilder
{
    private Inventory? _inventoryToClone;

    private InventoryId? _id;

    private ProductId? _productId;

    private Quantity? _quantity;

    private readonly IList<Adjustment> _adjustments = [];

    private readonly IList<Reservation> _reservations = [];

    public Result<Inventory> Build()
    {
        var id = _inventoryToClone?.Id ?? _id ?? new InventoryId(Guid.NewGuid());
        var productId = _productId ?? _inventoryToClone?.ProductId;
        var quantity = _quantity ?? _inventoryToClone?.Quantity ?? new Quantity(0);
        var adjustments = _adjustments.Count > 0 ? _adjustments.ToList().AsReadOnly() : _inventoryToClone?.Adjustments;
        var reservations =
            _reservations.Count > 0 ? _reservations.ToList().AsReadOnly() : _inventoryToClone?.Reservations;

        return Inventory.Create(
            id,
            productId,
            quantity,
            adjustments ?? [],
            reservations ?? [],
            _inventoryToClone?.GetDomainEvents().ToList()
        );
    }

    public InventoryBuilder WithInventoryToClone(Inventory inventoryToClone)
    {
        ArgumentNullException.ThrowIfNull(inventoryToClone);

        _inventoryToClone = inventoryToClone;
        return this;
    }

    public InventoryBuilder WithId(InventoryId id)
    {
        _id = id;
        return this;
    }

    public InventoryBuilder WithProductId(ProductId productId)
    {
        _productId = productId;
        return this;
    }

    public InventoryBuilder WithQuantity(Quantity quantity)
    {
        _quantity = quantity;
        return this;
    }

    public InventoryBuilder WithAdjustment(params IEnumerable<Adjustment> adjustments)
    {
        ArgumentNullException.ThrowIfNull(adjustments);

        foreach (var adjustment in adjustments)
            _adjustments.Add(adjustment);

        return this;
    }

    public InventoryBuilder WithReservations(params IEnumerable<Reservation> reservations)
    {
        ArgumentNullException.ThrowIfNull(reservations);

        foreach (var reservation in reservations)
            _reservations.Add(reservation);

        return this;
    }
}
