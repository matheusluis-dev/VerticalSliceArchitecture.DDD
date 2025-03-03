using Domain.Inventories.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

[UsedImplicitly]
public sealed class AdjustmentIdConverter : ValueConverter<AdjustmentId, GuidV7>
{
    public AdjustmentIdConverter()
        : base(adjustmentId => adjustmentId.Value, guid => new AdjustmentId(guid)) { }
}
