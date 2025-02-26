using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products.UpdateProduct;

public sealed record Response(ProductId Id, ProductName Name);
