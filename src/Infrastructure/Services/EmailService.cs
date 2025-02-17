namespace Infrastructure.Services;

using Domain.Common.ValueObjects;

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
