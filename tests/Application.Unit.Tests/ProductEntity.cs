namespace Application.Unit.Tests;

using Application.Domain.Products.Entities;
using Application.Domain.Products.ValueObjects;
using Shouldly;
using Vogen;

public sealed class ProductEntity
{
    [Fact]
    public void Should_not_allow_empty_name_product()
    {
        // Act
        Action action = () => new Product { Id = ProductId.Create(), Name = ProductName.From(" ") };

        // Assert
        action
            .ShouldThrow<ValueObjectValidationException>()
            .Message.ShouldBe("Product name must be defined.");
    }
}
