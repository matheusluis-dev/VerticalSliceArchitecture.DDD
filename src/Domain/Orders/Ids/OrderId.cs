namespace Domain.Orders.Ids;

public sealed class OrderId : TypedId<GuidV7>
{
    public OrderId(GuidV7 value)
        : base(value) { }
}
