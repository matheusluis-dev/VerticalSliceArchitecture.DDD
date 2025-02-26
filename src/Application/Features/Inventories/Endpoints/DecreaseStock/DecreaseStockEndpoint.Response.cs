using Domain.Inventories.Ids;
using Domain.Products.Ids;

namespace Application.Features.Inventories.Endpoints.DecreaseStock;

public static partial class DecreaseStockEndpoint
{
    internal sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
}
