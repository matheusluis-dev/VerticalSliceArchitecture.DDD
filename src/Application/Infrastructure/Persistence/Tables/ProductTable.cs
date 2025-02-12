namespace Application.Infrastructure.Persistence.Tables;

using Application.Domain.Inventories.ValueObjects;
using Application.Domain.Products.ValueObjects;

public sealed class ProductTable
{
    public required ProductId Id { get; set; }
    public InventoryId? InventoryId { get; set; }
    public ProductName Name { get; set; }

    public ICollection<OrderItemTable> OrderItems { get; set; }
    public InventoryTable? Inventory { get; set; }
}
