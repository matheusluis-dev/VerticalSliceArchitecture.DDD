using Domain.Inventories.Ids;

namespace Application.Features.Inventories.Endpoints.IncreaseStock;

public static partial class IncreaseStock
{
    internal sealed record Request(InventoryId Id, Quantity Quantity, string Reason);
}
