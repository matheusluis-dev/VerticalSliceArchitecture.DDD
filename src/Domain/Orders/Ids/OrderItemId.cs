namespace Domain.Orders.Ids;

public sealed class OrderItemId : TypedId<Guid>
{
    public OrderItemId(Guid value)
        : base(value) { }
}
