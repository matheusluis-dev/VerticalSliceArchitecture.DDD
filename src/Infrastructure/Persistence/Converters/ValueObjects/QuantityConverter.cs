using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.ValueObjects;

public sealed class QuantityConverter : ValueConverter<Quantity, int>
{
    public QuantityConverter()
        : base(quantity => quantity.Value, @int => new Quantity(@int)) { }
}
