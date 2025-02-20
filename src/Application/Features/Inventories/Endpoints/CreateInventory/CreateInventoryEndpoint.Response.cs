namespace Application.Features.Inventories.Endpoints.CreateInventory;

using Domain.Common.ValueObjects;
using Domain.Inventories.ValueObjects;
using Domain.Products.ValueObjects;

public static partial class CreateInventory
{
    public sealed record Response(InventoryId Id, ProductId ProductId, Quantity Quantity);
}
