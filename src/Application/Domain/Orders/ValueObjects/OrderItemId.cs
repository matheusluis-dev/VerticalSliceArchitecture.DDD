namespace Application.Domain.Orders.ValueObjects;

using Vogen;

[ValueObject<Guid>]
public readonly partial struct OrderItemId
{
    public static OrderItemId Create() => From(Guid.NewGuid());
}
