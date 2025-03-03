namespace Domain.Products.Ids;

public sealed class ProductId : TypedId<GuidV7>
{
    public ProductId(GuidV7 value)
        : base(value) { }
}
