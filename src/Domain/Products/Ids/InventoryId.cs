namespace Domain.Products.Ids;

public sealed class InventoryId : TypedId<Guid>
{
    public InventoryId(Guid value)
        : base(value) { }
}
