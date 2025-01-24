namespace Application.Domain.Common.Entities;

using Application.Domain.Common.Events;

public interface IHasDomainEvents
{
    DomainEvents DomainEvents { get; set; }
}
