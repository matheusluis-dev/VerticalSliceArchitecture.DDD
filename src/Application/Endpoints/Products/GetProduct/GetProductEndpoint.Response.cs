namespace Application.Endpoints.Products.GetProduct;

using Application.Domain.Products.ValueObjects;

public record Response(ProductId ProductId, ProductName ProductName);
