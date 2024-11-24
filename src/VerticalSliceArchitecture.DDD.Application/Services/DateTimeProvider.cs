namespace VerticalSliceArchitecture.DDD.Application.Services;

internal interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
