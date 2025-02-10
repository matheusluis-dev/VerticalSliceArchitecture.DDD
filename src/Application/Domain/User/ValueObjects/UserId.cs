namespace Application.Domain.User.ValueObjects;

using Vogen;

[ValueObject<Guid>(conversions: Conversions.Default | Conversions.EfCoreValueConverter)]
public readonly partial struct UserId
{
    public static UserId Create() => From(Guid.NewGuid());
}
