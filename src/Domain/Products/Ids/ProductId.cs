namespace Domain.Products.Ids;

public sealed class ProductId : TypedId<Guid>
{
    public ProductId(Guid value)
        : base(value) { }
}
