namespace Domain.Products.Ids;

public sealed class AdjustmentId : TypedId<Guid>
{
    public AdjustmentId(Guid value)
        : base(value) { }
}
