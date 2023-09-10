using System.Linq.Expressions;

namespace OFood.Shop.Domain.SeedWork;

public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity> GetAsync(Guid id, params Expression<Func<TEntity, object>>[] includeExpressions);

    Task<TEntity> GetAsync(Guid id, CancellationToken cancellationToken);
    IUnitOfWork UnitOfWork { get; }
}
