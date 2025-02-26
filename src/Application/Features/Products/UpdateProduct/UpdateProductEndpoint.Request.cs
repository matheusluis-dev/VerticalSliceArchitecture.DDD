using Domain.Products.Ids;
using Domain.Products.ValueObjects;

namespace Application.Features.Products.UpdateProduct;

public sealed record Request(ProductId Id, ProductName Name);
