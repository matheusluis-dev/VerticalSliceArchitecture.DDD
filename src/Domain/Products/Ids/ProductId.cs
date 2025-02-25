namespace Domain.Products.Ids;

using Domain.Common;

public sealed class ProductId : TypedId<Guid>
{
    public ProductId(Guid value)
        : base(value) { }
}
