namespace Application.Features.Inventories.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DomainEvents;
using Domain.Common.ValueObjects;
using Domain.Orders.Events;

public sealed class NotifyCommercialAreaWhenOrderCancelled
    : IDomainEventHandler<OrderCancelledEvent>
{
    private readonly EmailService _email;

    public NotifyCommercialAreaWhenOrderCancelled(EmailService email)
    {
        _email = email;
    }

    public async Task HandleAsync([NotNull] OrderCancelledEvent eventModel, CancellationToken ct)
    {
        await _email.SendEmailAsync(
            Email.From("system@system.com"),
            Email.From("commercial@system.com"),
            $"Order '{eventModel.Order.Id}' cancelled"
        );
    }
}
