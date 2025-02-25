namespace Application.Features.Inventories.Endpoints.IncreaseStock;

using Domain.Common.ValueObjects;
using Domain.Inventories.Ids;

public static partial class IncreaseStock
{
    public sealed record Request(InventoryId Id, Quantity Quantity, string Reason);
}
