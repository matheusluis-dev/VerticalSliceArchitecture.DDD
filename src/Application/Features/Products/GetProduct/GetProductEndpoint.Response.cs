namespace Application.Features.Products.GetProduct;

using Domain.Products.Ids;
using Domain.Products.ValueObjects;

public record Response(ProductId ProductId, ProductName ProductName);
