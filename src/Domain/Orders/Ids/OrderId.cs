namespace Domain.Orders.Ids;

using Domain.Common;

public sealed class OrderId : TypedId<Guid>
{
    public OrderId(Guid value)
        : base(value) { }
}
