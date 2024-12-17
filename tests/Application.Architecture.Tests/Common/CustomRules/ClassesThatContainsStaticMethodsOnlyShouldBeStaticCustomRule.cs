namespace Application.Architecture.Tests.Common.CustomRules;

using Mono.Cecil;

public sealed class ClassesThatContainsStaticMethodsOnlyShouldBeStaticCustomRule : ICustomRule2
{
    public CustomRuleResult MeetsRule([NotNull] TypeDefinition type)
    {
        if (type.FullName.Equals("Program", StringComparison.Ordinal))
        {
            return new CustomRuleResult(
                true,
                "Program entry point can not be considered in this test"
            );
        }

        if (type.IsStatic())
            return new CustomRuleResult(true, "Class is static");

        var staticMethods = type.GetStaticMethods();
        if (staticMethods.Length == 0)
            return new CustomRuleResult(true, "Class has not static methods");

        var nonStaticMethods = type.GetNonStaticMethods();

        var allMethodsAreStatic = nonStaticMethods.Length == 0;
        var message = allMethodsAreStatic
            ? "All methods are static, class is not static"
            : "Not all methods are static";

        return new CustomRuleResult(!allMethodsAreStatic, message);
    }
}
