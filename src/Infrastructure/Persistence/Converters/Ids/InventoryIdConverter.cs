using Domain.Inventories.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

[UsedImplicitly]
public sealed class InventoryIdConverter : ValueConverter<InventoryId, Guid>
{
    public InventoryIdConverter()
        : base(inventoryId => inventoryId.Value, guid => new InventoryId(guid)) { }
}
