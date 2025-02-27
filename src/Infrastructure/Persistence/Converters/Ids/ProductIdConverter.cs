using Domain.Products.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

public sealed class ProductIdConverter : ValueConverter<ProductId, Guid>
{
    public ProductIdConverter()
        : base(productId => productId.Value, guid => new ProductId(guid)) { }
}
