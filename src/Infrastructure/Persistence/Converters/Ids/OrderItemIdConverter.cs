using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

[UsedImplicitly]
public sealed class OrderItemIdConverter : ValueConverter<OrderItemId, GuidV7>
{
    public OrderItemIdConverter()
        : base(orderItemId => orderItemId.Value, guid => new OrderItemId(guid)) { }
}
