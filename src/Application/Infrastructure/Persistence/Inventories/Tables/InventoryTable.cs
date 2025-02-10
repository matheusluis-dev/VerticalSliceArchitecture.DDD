namespace Application.Infrastructure.Persistence.Inventories.Tables;

using Application.Domain.Common.ValueObjects;
using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Products.ValueObjects;
using Application.Infrastructure.Persistence.Products.Tables;

public sealed class InventoryTable
{
    public required InventoryId Id { get; set; }
    public required ProductId ProductId { get; set; }
    public Quantity QuantityAvailable { get; set; }

    public ProductTable Product { get; set; }
}
