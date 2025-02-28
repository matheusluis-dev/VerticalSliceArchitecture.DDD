using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.ValueObjects;

[UsedImplicitly]
public sealed class AmountConverter : ValueConverter<Amount, decimal>
{
    public AmountConverter()
        : base(amount => amount.Value, @decimal => new Amount(@decimal)) { }
}
