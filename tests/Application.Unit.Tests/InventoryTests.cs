using Ardalis.Result;
using Domain.Common.ValueObjects;
using Domain.Inventories.Aggregate;
using Domain.Inventories.Ids;
using Domain.Inventories.Services;
using Domain.Products.Entities;
using Domain.Products.ValueObjects;
using Shouldly;

namespace Application.Unit.Tests;

public sealed class InventoryTests
{
    [Fact]
    public void Can_not_create_inventory_if_product_already_has_one()
    {
        // Arrange
        var product = Product.Create(new ProductName("name")).Value!;
        var inventory = Inventory.Create(new InventoryId(Guid.NewGuid()), product.Id, new Quantity(1), [], []).Value;
        var productWithInventory = Product.Create(product.Name, product.Id, inventory);

        var sut = new CreateInventoryService();

        // Act
        var result = sut.CreateForProduct(productWithInventory, new Quantity(1));

        // Assert
        result.IsInvalid().ShouldBeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Can_not_create_inventory_with_quantity_lower_or_equal_zero(int quantity)
    {
        // Arrange
        var product = Product.Create(new ProductName("name")).Value!;
        var sut = new CreateInventoryService();

        // Act
        var result = sut.CreateForProduct(product, new Quantity(quantity));

        // Assert
        result.IsInvalid().ShouldBeTrue();
    }

    [Fact]
    public void Can_not_create_inventory_without_product()
    {
        // Arrange
        var builder = new InventoryBuilder().WithQuantity(new Quantity(1));

        // Act
        var result = builder.Build();

        // Assert
        result.IsInvalid().ShouldBeTrue();
    }
}
