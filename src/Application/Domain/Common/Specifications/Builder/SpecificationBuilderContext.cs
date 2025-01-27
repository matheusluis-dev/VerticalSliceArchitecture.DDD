namespace Application.Domain.Common.Specifications.Builder;

using System.Linq.Expressions;
using Application.Domain.Common.Entities;
using LinqKit;

public sealed class SpecificationBuilderContext<TEntity>
    where TEntity : class, IEntity
{
    private readonly List<List<ISpecification<TEntity>>> _groupedSpecifications = [];

    private void CreateGroupedSpecification()
    {
        _groupedSpecifications.Add([]);
    }

    internal void Or()
    {
        CreateGroupedSpecification();
    }

    public void AddSpecification(ISpecification<TEntity> specification)
    {
        if (_groupedSpecifications.Count == 0)
            CreateGroupedSpecification();

        _groupedSpecifications[^1].Add(specification);
    }

    public Expression<Func<TEntity, bool>> GetPredicate()
    {
        var predicate = PredicateBuilder.New<TEntity>(true);

        if (_groupedSpecifications.Count == 0)
            return predicate;

        foreach (var groupedSpecification in _groupedSpecifications)
        {
            var groupedPredicate = PredicateBuilder.New<TEntity>(true);
            foreach (var specification in groupedSpecification)
            {
                groupedPredicate = groupedPredicate.And(specification.Criteria);
            }

            predicate = predicate.Or(groupedPredicate);
        }

        return predicate;
    }
}
