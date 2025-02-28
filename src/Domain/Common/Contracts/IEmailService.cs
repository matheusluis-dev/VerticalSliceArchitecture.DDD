namespace Domain.Common.Contracts;

public interface IEmailService
{
    Task SendEmailAsync(Email to, Email from, string body);
}
