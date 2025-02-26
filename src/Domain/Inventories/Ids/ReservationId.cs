namespace Domain.Inventories.Ids;

public sealed class ReservationId : TypedId<Guid>
{
    public ReservationId(Guid value)
        : base(value) { }
}
