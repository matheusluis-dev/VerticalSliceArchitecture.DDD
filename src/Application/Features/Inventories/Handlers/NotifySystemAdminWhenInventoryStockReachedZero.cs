namespace Application.Features.Inventories.Handlers;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DomainEvents;
using Domain.Common.ValueObjects;
using Domain.Inventories.Events;

public sealed class NotifySystemAdminWhenInventoryStockReachedZero
    : IDomainEventHandler<InventoryStockReachedZeroEvent>
{
    private readonly IEmailService _email;

    public NotifySystemAdminWhenInventoryStockReachedZero(IEmailService email)
    {
        _email = email;
    }

    public async Task HandleAsync(
        [NotNull] InventoryStockReachedZeroEvent eventModel,
        CancellationToken ct
    )
    {
        await _email.SendEmailAsync(
            Email.From("system@system.com"),
            Email.From("admin@system.com"),
            $"Inventory '{eventModel.Id}' stock reached zero"
        );
    }
}
