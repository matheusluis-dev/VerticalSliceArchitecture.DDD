using Domain.Orders.Events;
using Domain.Orders.Ids;

namespace Application.Features.Inventories.Handlers.IntegrationEvents.OrderCancelled;

public sealed class NotifyCommercialAreaWhenOrderCancelled : IDomainEventHandler<OrderCancelledEvent>
{
    public Task HandleAsync(OrderCancelledEvent eventModel, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(eventModel);

        return new NotifyCommercialAreaWhenOrderCancelledCommand(eventModel.Order.Id).QueueJobAsync(ct: ct);
    }
}

public sealed record NotifyCommercialAreaWhenOrderCancelledCommand(OrderId OrderId) : ICommand;

public sealed class NotifyCommercialAreaWhenOrderCancelledCommandHandler
    : ICommandHandler<NotifyCommercialAreaWhenOrderCancelledCommand>
{
    private readonly EmailService _email;

    public NotifyCommercialAreaWhenOrderCancelledCommandHandler(EmailService email)
    {
        _email = email;
    }

    public Task ExecuteAsync(NotifyCommercialAreaWhenOrderCancelledCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        return _email.SendEmailAsync(
            new Email("system@system.com"),
            new Email("commercial@system.com"),
            $"Order '{command.OrderId}' was cancelled"
        );
    }
}
