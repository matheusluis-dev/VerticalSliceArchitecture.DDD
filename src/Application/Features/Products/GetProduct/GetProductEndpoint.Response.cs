namespace Application.Features.Products.GetProduct;

using Domain.Products.ValueObjects;

public record Response(ProductId ProductId, ProductName ProductName);
