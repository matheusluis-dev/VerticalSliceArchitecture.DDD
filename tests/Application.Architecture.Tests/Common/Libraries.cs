namespace Application.Architecture.Tests.Common;

using System.Collections;

internal sealed class Libraries : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { ("Newtonsoft.Json", "System.Text.Json") };

        yield return new object[]
        {
            ("System.IdentityModel.Tokens.Jwt", "Microsoft.AspNetCore.Authentication.JwtBearer"),
        };
        yield return new object[] { ("System.Web.Http", "Microsoft.AspNetCore.*") };

        yield return new object[]
        {
            ("System.Configuration", "Microsoft.Extensions.Configuration"),
        };

        yield return new object[] { ("System.Xml.Serialization", "System.Xml.Linq") };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
