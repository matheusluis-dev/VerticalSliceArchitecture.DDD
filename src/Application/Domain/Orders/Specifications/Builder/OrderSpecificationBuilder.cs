namespace Application.Domain.Orders.Specifications.Builder;

using Application.Domain.Common.Specifications.Builder;
using Application.Domain.Orders.Entities;

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
        decimal @value
    )
    {
        _context.AddSpecification(new PriceHigherThanValueSpecification(@value));
        return this;
    }
}
