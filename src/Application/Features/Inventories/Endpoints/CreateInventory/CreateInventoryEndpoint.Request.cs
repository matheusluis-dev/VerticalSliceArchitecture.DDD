using Domain.Products.Ids;

namespace Application.Features.Inventories.Endpoints.CreateInventory;

public static partial class CreateInventoryEndpoint
{
    internal sealed record Request(ProductId ProductId, Quantity Quantity);
}
