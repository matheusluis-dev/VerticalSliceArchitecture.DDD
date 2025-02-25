namespace Application.Features.Products.UpdateProduct;

using Domain.Products.Ids;
using Domain.Products.ValueObjects;

public sealed record Request(ProductId Id, ProductName Name);
