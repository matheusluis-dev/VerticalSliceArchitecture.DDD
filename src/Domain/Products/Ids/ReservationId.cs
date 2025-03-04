namespace Domain.Products.Ids;

public sealed class ReservationId : TypedId<Guid>
{
    public ReservationId(Guid value)
        : base(value) { }
}
