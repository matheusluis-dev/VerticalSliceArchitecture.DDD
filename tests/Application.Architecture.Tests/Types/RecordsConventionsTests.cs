namespace Application.Architecture.Tests.Types;

public sealed class RecordsConventionsTests
{
    [Fact]
    public void Records_should_be_PascalCased()
    {
        // Arrange
        var rules = SutArchGuard.Types.That.AreRecords().Should.HaveNamePascalCased();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Check.That(result).IsSuccess();
    }

    [Fact]
    public void Non_abstract_records_must_be_Sealed()
    {
        // Arrange
        var rules = SutArchGuard.Types.That.AreRecords().And.AreNotAbstract().Should.BeSealed();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Records_must_be_immutable()
    {
        // Arrange
        var rules = SutArchGuard.Types.That.AreRecords().Should.BeImmutable();

        // Act
        var result = rules.GetResult(StringComparison.Ordinal);

        // Assert
        Assert.True(result.IsSuccessful);
    }
}
