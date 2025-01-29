namespace Application.Domain.Orders.ValueObjects;

using Vogen;

[ValueObject<Guid>]
public readonly partial struct OrderId
{
    public static OrderId Create() => From(Guid.NewGuid());
}
