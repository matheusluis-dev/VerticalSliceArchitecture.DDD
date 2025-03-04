using Domain.Common.ValueObjects;
using Domain.Products.Aggregate;
using Domain.Products.ValueObjects;

namespace Application.Unit.Tests;

public sealed class InventoryTests
{
    [Fact]
    public void Can_not_create_inventory_if_product_already_has_one()
    {
        // Arrange
        var product = ProductBuilder
            .Start()
            .WithNewId()
            .WithName(new ProductName("name"))
            .Build()
            .Object!.CreateInventory(new Quantity(1))
            .Object!;

        // Act
        var result = product.CreateInventory(new Quantity(1));

        // Assert
        result.Failed.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Can_not_create_inventory_with_quantity_lower_or_equal_zero(int quantity)
    {
        // Arrange
        var product = ProductBuilder.Start().WithNewId().WithName(new ProductName("name")).Build().Object!;

        // Act
        Action act = () => product.CreateInventory(new Quantity(quantity));

        // Assert
        act.ShouldThrow<QuantityException>();
    }
}
