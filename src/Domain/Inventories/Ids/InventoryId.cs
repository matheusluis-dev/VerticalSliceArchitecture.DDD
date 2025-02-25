namespace Domain.Inventories.Ids;

using Domain.Common;

public sealed class InventoryId : TypedId<Guid>
{
    public InventoryId(Guid value)
        : base(value) { }
}
