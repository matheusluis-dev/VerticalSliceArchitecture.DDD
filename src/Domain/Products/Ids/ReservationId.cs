namespace Domain.Products.Ids;

public sealed class ReservationId : TypedId<GuidV7>
{
    public ReservationId(GuidV7 value)
        : base(value) { }
}
