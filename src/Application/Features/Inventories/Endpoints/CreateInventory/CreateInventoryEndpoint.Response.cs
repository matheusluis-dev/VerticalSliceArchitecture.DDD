using Domain.Inventories.Ids;
using Domain.Products.Ids;

namespace Application.Features.Inventories.Endpoints.CreateInventory;

public static partial class CreateInventoryEndpoint
{
    internal sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
}
