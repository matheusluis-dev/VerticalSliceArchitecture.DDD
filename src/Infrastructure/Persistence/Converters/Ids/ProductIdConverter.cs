using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

[UsedImplicitly]
public sealed class ProductIdConverter : ValueConverter<ProductId, GuidV7>
{
    public ProductIdConverter()
        : base(productId => productId.Value, guid => new ProductId(guid)) { }
}
