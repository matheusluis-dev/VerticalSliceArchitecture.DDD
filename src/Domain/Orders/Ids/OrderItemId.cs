namespace Domain.Orders.Ids;

public sealed class OrderItemId : TypedId<GuidV7>
{
    public OrderItemId(GuidV7 value)
        : base(value) { }
}
