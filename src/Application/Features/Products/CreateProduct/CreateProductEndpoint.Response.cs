namespace Application.Features.Products.CreateProduct;

using Domain.Products.ValueObjects;

public static partial class CreateProduct
{
    public sealed record Response(ProductId Id, ProductName Name);
}
