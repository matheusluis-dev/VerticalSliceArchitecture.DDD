namespace Application.Features.Inventories.Handlers.IntegrationEvents.OrderCancelledEvent;

using Domain.Common.DomainEvents;
using Domain.Common.ValueObjects;
using Domain.Orders.Events;
using Domain.Orders.ValueObjects;

public sealed class NotifyCommercialAreaWhenOrderCancelled : IDomainEventHandler<OrderCancelledEvent>
{
    public Task HandleAsync([NotNull] OrderCancelledEvent eventModel, CancellationToken ct)
    {
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

    public Task ExecuteAsync([NotNull] NotifyCommercialAreaWhenOrderCancelledCommand command, CancellationToken ct)
    {
        return _email.SendEmailAsync(
            Email.From("system@system.com"),
            Email.From("commercial@system.com"),
            $"Order '{command.OrderId}' was cancelled"
        );
    }
}
