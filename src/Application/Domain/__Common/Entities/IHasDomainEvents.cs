namespace Application.Domain.__Common.Entities;

using Application.Domain.__Common.Events;

public interface IHasDomainEvents
{
    DomainEvents DomainEvents { get; set; }
}
