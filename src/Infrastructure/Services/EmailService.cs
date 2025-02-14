namespace Infrastructure.Services;

using Domain.Common.ValueObjects;

public interface IEmailService
{
    Task SendEmailAsync(Email to, Email from, string body);
}

public sealed class EmailService : IEmailService
{
    public async Task SendEmailAsync(Email to, Email from, string body)
    {
        await Task.CompletedTask;
    }
}
