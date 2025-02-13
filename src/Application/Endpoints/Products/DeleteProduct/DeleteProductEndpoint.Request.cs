namespace Application.Endpoints.Products.DeleteProduct;

using Application.Domain.Products.ValueObjects;

public sealed record Request(ProductId Id);
