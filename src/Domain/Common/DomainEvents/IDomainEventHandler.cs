using System.Diagnostics.CodeAnalysis;

namespace Domain.Common.DomainEvents;

[SuppressMessage(
    "Design",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "The name 'IDomainEventHandler' is intentional and reflects its purpose."
)]
public interface IDomainEventHandler<TEvent> : FastEndpoints.IEventHandler<TEvent>
    where TEvent : class, IDomainEvent;
