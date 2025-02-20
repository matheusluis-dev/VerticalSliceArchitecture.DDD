namespace Application.Features.Products.DeleteProduct;

using Domain.Products.ValueObjects;

public static partial class DeleteProduct
{
    public sealed record Request(ProductId Id);
}
