namespace Application.Endpoints.Products.CreateProduct;

using Application.Domain.Products.ValueObjects;

public sealed record Response(ProductId Id, ProductName Name);
