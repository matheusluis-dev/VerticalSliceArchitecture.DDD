namespace Application.Features.Inventories.Endpoints.CreateInventory;

using Domain.Common.ValueObjects;
using Domain.Products.Ids;

public static partial class CreateInventory
{
    public sealed record Request(ProductId ProductId, Quantity Quantity);
}
