using Domain.Common.ValueObjects;

namespace Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    public Task SendEmailAsync(Email to, Email from, string body)
    {
        return Task.CompletedTask;
    }
}
