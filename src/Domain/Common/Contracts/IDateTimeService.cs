namespace Domain.Common.Contracts;

public interface IDateTimeService
{
    public DateTimeOffset UtcNow { get; }
}
