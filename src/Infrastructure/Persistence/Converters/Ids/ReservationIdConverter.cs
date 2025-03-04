using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

[UsedImplicitly]
public sealed class ReservationIdConverter : ValueConverter<ReservationId, Guid>
{
    public ReservationIdConverter()
        : base(reservationId => reservationId.Value, guid => new ReservationId(guid)) { }
}
