namespace Domain.Orders.Ids;

using Domain.Common;

public sealed class OrderItemId : TypedId<Guid>
{
    public OrderItemId(Guid value)
        : base(value) { }
}
