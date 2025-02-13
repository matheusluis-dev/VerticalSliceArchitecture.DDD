namespace Application.Endpoints.Products.UpdateProduct;

using Application.Domain.Products.ValueObjects;

public sealed record Response(ProductId Id, ProductName Name);
