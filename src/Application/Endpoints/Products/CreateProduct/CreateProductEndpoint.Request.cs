namespace Application.Endpoints.Products.CreateProduct;

using Application.Domain.Products.ValueObjects;

public sealed record Request(ProductName Name);
