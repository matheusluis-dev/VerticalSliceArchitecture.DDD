using Domain.Inventories.Ids;
using Domain.Products.Ids;

namespace Application.Features.Inventories.Endpoints.IncreaseStock;

public static partial class IncreaseStock
{
    internal sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
}
