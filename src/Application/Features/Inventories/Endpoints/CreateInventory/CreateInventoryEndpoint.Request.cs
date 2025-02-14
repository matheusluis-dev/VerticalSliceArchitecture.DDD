namespace Application.Features.Inventories.Endpoints.CreateInventory;

using Domain.Common.ValueObjects;
using Domain.Products.ValueObjects;

public sealed record Request(ProductId ProductId, Quantity Quantity);
