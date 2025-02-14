namespace Application.Features.Products.DeleteProduct;

using Domain.Products.ValueObjects;

public sealed record Request(ProductId Id);
