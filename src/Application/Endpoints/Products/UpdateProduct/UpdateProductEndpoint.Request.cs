namespace Application.Endpoints.Products.UpdateProduct;

using Application.Domain.Products.ValueObjects;

public sealed record Request(ProductId Id, ProductName Name);
