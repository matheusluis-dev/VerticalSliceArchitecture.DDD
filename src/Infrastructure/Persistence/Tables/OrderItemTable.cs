namespace Infrastructure.Persistence.Tables;

using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Orders.ValueObjects;
using Domain.Products.ValueObjects;

public sealed class OrderItemTable
{
    public OrderId OrderId { get; set; }
    public OrderItemId Id { get; set; }
    public ProductId ProductId { get; set; }
    public ReservationId ReservationId { get; set; }
    public Quantity Quantity { get; set; }
    public Amount UnitPrice { get; set; }

    public OrderTable Order { get; set; }
    public ProductTable Product { get; set; }
    public ReservationTable Reservation { get; set; }
}
