namespace Application.Endpoints.Inventories.IncreaseStock;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Products.ValueObjects;

public sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
