namespace Application.Domain.Orders.Specifications.Builder;

using Application.Domain.Common.Specifications.Builder;
using Application.Domain.Common.ValueObjects;
using Application.Domain.Orders.Aggregates;

public sealed class OrderSpecificationBuilder
    : SpecificationBuilder<Order, IOrderSpecificationBuilderCriteria>,
        IOrderSpecificationBuilderCriteria
{
    public ISpecificationSequence<Order, IOrderSpecificationBuilderCriteria> ArePaid()
    {
        _context.AddSpecification(new ArePaidSpecification());
        return this;
    }

    public ISpecificationSequence<Order, IOrderSpecificationBuilderCriteria> TotalPriceHigherThan(
        Amount amount
    )
    {
        _context.AddSpecification(new PriceHigherThanValueSpecification(amount));
        return this;
    }
}
