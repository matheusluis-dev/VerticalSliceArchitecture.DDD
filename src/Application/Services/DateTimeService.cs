namespace Application.Services;

using Microsoft.Extensions.Internal;

public sealed class DateTimeService : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
