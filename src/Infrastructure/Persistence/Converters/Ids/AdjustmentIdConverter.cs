using Domain.Inventories.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

public sealed class AdjustmentIdConverter : ValueConverter<AdjustmentId, Guid>
{
    public AdjustmentIdConverter()
        : base(adjustmentId => adjustmentId.Value, guid => new AdjustmentId(guid)) { }
}
