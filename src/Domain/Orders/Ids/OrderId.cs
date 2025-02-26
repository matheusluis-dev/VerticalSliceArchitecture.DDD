namespace Domain.Orders.Ids;

public sealed class OrderId : TypedId<Guid>
{
    public OrderId(Guid value)
        : base(value) { }
}
