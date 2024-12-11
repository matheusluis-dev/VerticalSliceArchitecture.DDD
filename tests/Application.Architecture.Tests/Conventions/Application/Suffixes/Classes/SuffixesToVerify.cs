namespace Application.Architecture.Tests.Conventions.Application.Suffixes.Classes;

using System.Collections;

public class SuffixesToVerify : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { (".Dtos", "Dto") };
        yield return new object[] { (".Services", "Service") };
        yield return new object[] { (".Aggregates", "Aggregate") };
        yield return new object[] { (".Repositories", "Repository") };
        yield return new object[] { (".Extensions", "Extensions") };
        yield return new object[] { (".Configuration", "Configuration") };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
