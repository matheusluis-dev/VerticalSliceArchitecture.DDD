namespace Application.Domain.Common.Specifications;

using Application.Domain.Common.Entities;
using LinqKit;

public sealed class SpecificationContext<TEntity> where TEntity : class, IEntity
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

    public IQueryable<TEntity> GetQueryable()
    {
        PredicateBuilder.
    }
}
