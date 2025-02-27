using Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters.ValueObjects;

public sealed class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter()
        : base(email => email.Value, @string => new Email(@string)) { }
}
