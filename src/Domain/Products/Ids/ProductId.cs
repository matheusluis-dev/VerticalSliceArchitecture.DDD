namespace Domain.Products.Ids;

public sealed class ProductId : TypedId<Guid>
{
    public ProductId()
        : this(Guid.Empty) { }

    public ProductId(Guid value)
        : base(value) { }
}
