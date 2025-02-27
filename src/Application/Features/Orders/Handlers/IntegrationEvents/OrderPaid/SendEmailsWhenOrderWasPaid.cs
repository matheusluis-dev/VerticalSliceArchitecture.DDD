using Domain.Orders.Events;
using Domain.Orders.Ids;

namespace Application.Features.Orders.Handlers.IntegrationEvents.OrderPaid;

public sealed class SendEmailsWhenOrderWasPaid : IEventHandler<OrderPaidEvent>
{
    public Task HandleAsync(OrderPaidEvent eventModel, CancellationToken ct)
    {
        return new SendEmailsWhenOrderWasPaidCommand(eventModel.Order.Id, eventModel.Order.CustomerEmail).QueueJobAsync(
            ct: ct
        );
    }
}

public sealed record SendEmailsWhenOrderWasPaidCommand(OrderId OrderId, Email CustomerEmail) : ICommand;

public sealed class SendEmailsWhenOrderWasPaidCommandHandler : ICommandHandler<SendEmailsWhenOrderWasPaidCommand>
{
    private readonly EmailService _email;

    public SendEmailsWhenOrderWasPaidCommandHandler(EmailService emailService)
    {
        _email = emailService;
    }

    public Task ExecuteAsync(SendEmailsWhenOrderWasPaidCommand command, CancellationToken ct)
    {
        var sendEmailToCostumer = _email.SendEmailAsync(
            command.CustomerEmail,
            new Email("system@system.com"),
            $"Order {command.OrderId} paid successfully"
        );

        var sendEmailToCommercialArea = _email.SendEmailAsync(
            new Email("commercial@system.com"),
            new Email("system@system.com"),
            $"Order {command.OrderId} paid successfully"
        );

        return Task.WhenAll(sendEmailToCostumer, sendEmailToCommercialArea);
    }
}
