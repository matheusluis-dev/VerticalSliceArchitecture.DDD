namespace Application.Features.Inventories.Endpoints.IncreaseStock;

using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Products.ValueObjects;

public static partial class IncreaseStock
{
    public sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
}
