namespace Application.Endpoints.Inventories.CreateInventory;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Products.ValueObjects;

public sealed record Request(ProductId ProductId, Quantity Quantity);
