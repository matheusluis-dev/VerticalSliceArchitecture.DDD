using FluentAssertions;

namespace Application.Unit.Tests;

public sealed class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var num1 = 10;
        var num2 = 20;

        // Act
        var sum = num1 + num2;

        // Assert
        sum.Should().Be(num1);
    }
}
