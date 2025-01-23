namespace Application.Infrastructure.Services;

public sealed class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

public interface IDateTimeService
{
    public DateTimeOffset UtcNow { get; }
}
