using Domain.Inventories.Events;
using Domain.Inventories.Ids;
using Domain.Products.Ids;

namespace Application.Features.Inventories.Handlers.IntegrationEvents.InventoryStockReachedZero;

public sealed class NotifySystemAdminWhenInventoryStockReachedZero : IDomainEventHandler<InventoryStockReachedZeroEvent>
{
    public Task HandleAsync(InventoryStockReachedZeroEvent eventModel, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(eventModel);

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

    public Task ExecuteAsync(NotifySystemAdminWhenInventoryStockReachedZeroCommand command, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(command);

        return _email.SendEmailAsync(
            new Email("system@system.com"),
            new Email("admin@system.com"),
            $"Inventory '{command.InventoryId}' from product '{command.ProductId}' stock reached zero"
        );
    }
}
