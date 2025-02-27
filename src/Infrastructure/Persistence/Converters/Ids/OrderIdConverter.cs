using Domain.Orders.Ids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.Ids;

public sealed class OrderIdConverter : ValueConverter<OrderId, Guid>
{
    public OrderIdConverter()
        : base(orderId => orderId.Value, guid => new OrderId(guid)) { }
}
