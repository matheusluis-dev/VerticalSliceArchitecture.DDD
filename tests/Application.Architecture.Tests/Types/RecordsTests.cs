namespace Application.Architecture.Tests.Types;

public sealed class RecordsTests
{
    [Fact]
    public void Records_should_have_name_PascalCased()
    {
        // Arrange
        var rules = SystemUnderTest.Types.That.AreRecords().Should.HaveNamePascalCased();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }

    [Fact]
    public void Non_abstract_records_should_be_sealed()
    {
        // Arrange
        var rules = SystemUnderTest.Types.That.AreRecords().And.AreNotAbstract().Should.BeSealed();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Records_should_be_immutable()
    {
        // Arrange
        var rules = SystemUnderTest.Types.That.AreRecords().Should.BeImmutable();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
