namespace Application.Domain.Common.Repositories;

using Application.Domain.Common.Entities;
using Application.Domain.Common.Specifications;

public interface IRepository<TEntity, TSpecification>
    where TEntity : class, IEntity
    where TSpecification : class, ISpecification<TEntity>
{

}
