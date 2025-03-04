using Domain.Products.Aggregate;
using Domain.Products.ValueObjects;

namespace Application.Unit.Tests;

public sealed class ProductTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Should_not_allow_empty_name_product(string name)
    {
        // Act
        Action action = () => ProductBuilder.Start().WithNewId().WithName(new ProductName(name)).Build();

        // Assert
        action.ShouldThrow<ArgumentException>().ParamName.ShouldBe("name");
    }

    [Fact]
    public void Should_not_allow_update_name_to_same_name()
    {
        // Arrange
        var name = new ProductName("ProductName");
        var product = ProductBuilder.Start().WithNewId().WithName(name).Build().Object!;

        // Act
        var result = product.UpdateName(name);

        // Assert
        result.Failed.ShouldBeTrue();
    }
}
