namespace Application.Features.Inventories.Endpoints.IncreaseStock;

using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;
using Domain.Products.Ids;

public static partial class IncreaseStock
{
    public sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
}
