namespace Application.Infrastructure.Persistence.Tables;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Orders.ValueObjects;
using Application.Domain.Products.ValueObjects;

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
