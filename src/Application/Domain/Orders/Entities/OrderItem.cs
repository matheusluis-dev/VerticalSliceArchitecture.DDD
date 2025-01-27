namespace Application.Domain.Orders.Entities;

using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.ValueObjects;

public sealed class OrderItem : IChildEntity, IAuditable
{
    public OrderItemId Id { get; init; }
    public OrderId OrderId { get; init; }
    public Quantity Quantity { get; set; }
    public Amount UnitPrice { get; set; }
    public Amount Price => UnitPrice.MultiplyByQuantity(Quantity);

    public DateTime Created { get; set; }
    public UserName CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserName LastModifiedBy { get; set; }

    private OrderItem() { }

    public OrderItem(OrderItemId id, OrderId orderId, Quantity quantity, Amount unitPrice)
    {
        Id = id;
        OrderId = orderId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
