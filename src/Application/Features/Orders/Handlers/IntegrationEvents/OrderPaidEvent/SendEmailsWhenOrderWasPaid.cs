namespace Application.Features.Orders.Handlers.IntegrationEvents.OrderPaidEvent;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.ValueObjects;
using Domain.Orders.Events;
using Domain.Orders.ValueObjects;

public sealed class SendEmailsWhenOrderWasPaid : IEventHandler<OrderPaidEvent>
{
    public Task HandleAsync([NotNull] OrderPaidEvent eventModel, CancellationToken ct)
    {
        return new SendEmailsWhenOrderWasPaidCommand(
            eventModel.Order.Id,
            eventModel.Order.CustomerEmail
        ).QueueJobAsync(ct: ct);
    }
}

public sealed record SendEmailsWhenOrderWasPaidCommand(OrderId OrderId, Email CustomerEmail)
    : ICommand;

public sealed class SendEmailsWhenOrderWasPaidCommandHandler
    : ICommandHandler<SendEmailsWhenOrderWasPaidCommand>
{
    private readonly EmailService _email;

    public SendEmailsWhenOrderWasPaidCommandHandler(EmailService emailService)
    {
        _email = emailService;
    }

    public Task ExecuteAsync(
        [NotNull] SendEmailsWhenOrderWasPaidCommand command,
        CancellationToken ct
    )
    {
        var sendEmailToCostumer = _email.SendEmailAsync(
            command.CustomerEmail,
            Email.From("system@system.com"),
            $"Order {command.OrderId} paid successfully"
        );

        var sendEmailToCommercialArea = _email.SendEmailAsync(
            Email.From("commercial@system.com"),
            Email.From("system@system.com"),
            $"Order {command.OrderId} paid successfully"
        );

        return Task.WhenAll(sendEmailToCostumer, sendEmailToCommercialArea);
    }
}
