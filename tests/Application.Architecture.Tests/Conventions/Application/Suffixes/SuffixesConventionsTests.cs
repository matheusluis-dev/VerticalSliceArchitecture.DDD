namespace Application.Architecture.Tests.Conventions.Application.Suffixes;

public sealed class SuffixesConventionsTests
{
    [Theory]
    [ClassData(typeof(Classes.SuffixesToVerify))]
    public void Classes_inside_namespace_ending_with_a_suffix_should_have_a_suffix(
        (string NamespaceSuffix, string ClassSuffix) p
    )
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .ResideInNamespaceEndingWith(p.NamespaceSuffix)
            .Should()
            .HaveNameEndingWith(p.ClassSuffix);

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful(
            $"Objects inside namespaces ending with '{p.NamespaceSuffix}' should have '{p.ClassSuffix}' suffix"
        );
    }

    [Theory]
    [ClassData(typeof(Classes.SuffixesToVerify))]
    public void Classes_outside_namespace_Dtos_should_not_have_Dto_suffix(
        (string NamespaceSuffix, string ClassSuffix) p
    )
    {
        // Arrange
        var rules = Solution
            .AllTypes.That()
            .DoNotResideInNamespaceEndingWith(p.NamespaceSuffix)
            .Should()
            .NotHaveNameEndingWith(p.ClassSuffix);

        // Act
        var result = rules.GetResult();

        // Assert
        result.ShouldBeSuccessful(
            $"Objects not inside namespaces ending with '{p.NamespaceSuffix}' should not have '{p.ClassSuffix}' suffix"
        );
    }
}
