namespace Application.Features.Inventories.Handlers.IntegrationEvents.InventoryStockReachedZeroEvent;

using System.Threading;
using System.Threading.Tasks;
using Domain.Common.DomainEvents;
using Domain.Common.ValueObjects;
using Domain.Inventories.Events;
using Domain.Inventories.Ids;
using Domain.Products.Ids;

public sealed class NotifySystemAdminWhenInventoryStockReachedZero : IDomainEventHandler<InventoryStockReachedZeroEvent>
{
    public Task HandleAsync([NotNull] InventoryStockReachedZeroEvent eventModel, CancellationToken ct)
    {
        return new NotifySystemAdminWhenInventoryStockReachedZeroCommand(
            eventModel.Inventory.Id,
            eventModel.ProductId
        ).QueueJobAsync(ct: ct);
    }
}

public sealed record NotifySystemAdminWhenInventoryStockReachedZeroCommand(InventoryId InventoryId, ProductId ProductId)
    : ICommand;

public sealed class NotifySystemAdminWhenInventoryStockReachedZeroCommandHandler
    : ICommandHandler<NotifySystemAdminWhenInventoryStockReachedZeroCommand>
{
    private readonly EmailService _email;

    public NotifySystemAdminWhenInventoryStockReachedZeroCommandHandler(EmailService email)
    {
        _email = email;
    }

    public Task ExecuteAsync(
        [NotNull] NotifySystemAdminWhenInventoryStockReachedZeroCommand command,
        CancellationToken ct
    )
    {
        return _email.SendEmailAsync(
            new Email("system@system.com"),
            new Email("admin@system.com"),
            $"Inventory '{command.InventoryId}' from product '{command.ProductId}' stock reached zero"
        );
    }
}
