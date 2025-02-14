namespace Infrastructure.Services;

public interface IDateTimeService
{
    public DateTimeOffset UtcNow { get; }
}

public sealed class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
