namespace Infrastructure.Persistence.Tables;

using Domain.Common.ValueObjects;
using Domain.Orders.Enums;
using Domain.Orders.ValueObjects;

public sealed class OrderTable
{
    public OrderId Id { get; set; }
    public Email CustomerEmail { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime? CanceledDate { get; set; }

    public ICollection<OrderItemTable> OrderItems { get; set; }
}
