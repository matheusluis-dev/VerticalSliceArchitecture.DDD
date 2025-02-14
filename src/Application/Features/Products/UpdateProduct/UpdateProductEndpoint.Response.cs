namespace Application.Features.Products.UpdateProduct;

using Domain.Products.ValueObjects;

public sealed record Response(ProductId Id, ProductName Name);
