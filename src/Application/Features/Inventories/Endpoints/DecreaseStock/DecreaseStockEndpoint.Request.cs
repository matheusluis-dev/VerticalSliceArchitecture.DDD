using Domain.Inventories.Ids;

namespace Application.Features.Inventories.Endpoints.DecreaseStock;

public static partial class DecreaseStockEndpoint
{
    internal sealed record Request(InventoryId Id, Quantity Quantity, string Reason);
}
