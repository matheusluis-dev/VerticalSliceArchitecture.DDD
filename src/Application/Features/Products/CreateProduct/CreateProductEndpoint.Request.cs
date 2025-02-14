namespace Application.Features.Products.CreateProduct;

using Domain.Products.ValueObjects;

public sealed record Request(ProductName Name);
