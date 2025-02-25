namespace Application.Unit.Tests;

using Ardalis.Result;
using Domain.Products.Entities;
using Domain.Products.ValueObjects;
using Shouldly;

public sealed class ProductTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Should_not_allow_empty_name_product(string name)
    {
        // Act
        var result = Product.Create(new ProductName(name));

        // Assert
        result.IsInvalid().ShouldBeTrue();
    }

    [Fact]
    public void Should_not_allow_update_name_to_same_name()
    {
        // Arrange
        var name = new ProductName("ProductName");
        var product = Product.Create(name).Value;

        // Act
        var result = product.UpdateName(name);

        // Assert
        result.IsInvalid().ShouldBeTrue();
    }
}
