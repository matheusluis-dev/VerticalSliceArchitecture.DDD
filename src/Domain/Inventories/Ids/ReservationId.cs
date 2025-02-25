namespace Domain.Inventories.Ids;

using Domain.Common;

public sealed class ReservationId : TypedId<Guid>
{
    public ReservationId(Guid value)
        : base(value) { }
}
