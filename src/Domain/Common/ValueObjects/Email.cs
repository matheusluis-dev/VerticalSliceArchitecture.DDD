using System.Text.RegularExpressions;

namespace Domain.Common.ValueObjects;

public sealed partial class Email : ValueObject
{
    public string Value { get; init; } = null!;

    public Email() { }

    public Email(string address)
    {
        if (!EmailRegex().IsMatch(address))
            throw EmailException.InvalidFormat(address);

        Value = address;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    [GeneratedRegex(
        """(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])""",
        RegexOptions.Compiled | RegexOptions.ExplicitCapture,
        1_000
    )]
    private static partial Regex EmailRegex();
}

public sealed class EmailException : Exception
{
    private EmailException(string message)
        : base(message) { }

    internal static EmailException InvalidFormat(string emailAddress)
    {
        return new EmailException($"'{emailAddress}' has an invalid format");
    }
}
