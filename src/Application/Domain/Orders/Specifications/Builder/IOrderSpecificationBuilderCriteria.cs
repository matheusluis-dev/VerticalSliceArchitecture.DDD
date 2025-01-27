namespace Application.Domain.Orders.Specifications.Builder;

using Application.Domain.Common.Specifications.Builder;
using Application.Domain.Orders.Aggregates;

public interface IOrderSpecificationBuilderCriteria : ISpecificationCriteria
{
    ISpecificationSequence<Order, IOrderSpecificationBuilderCriteria> ArePaid();
    ISpecificationSequence<Order, IOrderSpecificationBuilderCriteria> TotalPriceHigherThan(
        decimal @value
    );
}
