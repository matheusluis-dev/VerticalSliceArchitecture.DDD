namespace Domain.Inventories.Ids;

public sealed class AdjustmentId : TypedId<Guid>
{
    public AdjustmentId(Guid value)
        : base(value) { }
}
