using Domain.Common.ValueObjects;

namespace Infrastructure.Services;

public interface IEmailService
{
    Task SendEmailAsync(Email to, Email from, string body);
}

public sealed class EmailService : IEmailService
{
    public Task SendEmailAsync(Email to, Email from, string body)
    {
        return Task.CompletedTask;
    }
}
