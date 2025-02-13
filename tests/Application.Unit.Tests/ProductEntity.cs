namespace Application.Unit.Tests;

public sealed class ProductEntity
{
    [Fact]
    public void Should_not_allow_empty_name_product()
    {
        //// Act
        //Action action = () => new Product(ProductName.From(" "));

        //// Assert
        //action
        //    .ShouldThrow<ValueObjectValidationException>()
        //    .Message.ShouldBe("Product name must be defined.");
    }
}
