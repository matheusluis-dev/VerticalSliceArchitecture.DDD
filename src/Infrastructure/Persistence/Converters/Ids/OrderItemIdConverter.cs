using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

public sealed class OrderItemIdConverter : ValueConverter<OrderItemId, Guid>
{
    public OrderItemIdConverter()
        : base(orderItemId => orderItemId.Value, guid => new OrderItemId(guid)) { }
}
