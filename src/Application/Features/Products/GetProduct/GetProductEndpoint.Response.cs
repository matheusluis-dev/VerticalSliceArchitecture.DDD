using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products.GetProduct;

public record Response(ProductId ProductId, ProductName ProductName);
