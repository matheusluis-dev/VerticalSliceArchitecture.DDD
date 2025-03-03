namespace Domain.Inventories.Ids;

public sealed class AdjustmentId : TypedId<GuidV7>
{
    public AdjustmentId(GuidV7 value)
        : base(value) { }
}
