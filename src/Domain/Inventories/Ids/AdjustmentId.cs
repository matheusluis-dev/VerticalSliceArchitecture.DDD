namespace Domain.Inventories.Ids;

using Domain.Common;

public sealed class AdjustmentId : TypedId<Guid>
{
    public AdjustmentId(Guid value)
        : base(value) { }
}
