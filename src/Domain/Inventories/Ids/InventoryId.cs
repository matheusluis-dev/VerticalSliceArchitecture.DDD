namespace Domain.Inventories.Ids;

public sealed class InventoryId : TypedId<GuidV7>
{
    public InventoryId(GuidV7 value)
        : base(value) { }
}
