namespace Application.Features.Inventories.Endpoints.DecreaseStock;

using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;
using Domain.Products.Ids;

public sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
