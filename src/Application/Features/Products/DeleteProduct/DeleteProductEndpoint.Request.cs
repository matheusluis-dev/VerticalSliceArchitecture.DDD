namespace Application.Features.Products.DeleteProduct;

using Domain.Products.Ids;

public static partial class DeleteProduct
{
    public sealed record Request(ProductId Id);
}
