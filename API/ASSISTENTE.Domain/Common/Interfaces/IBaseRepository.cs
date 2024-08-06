using System.Linq.Expressions;
using ASSISTENTE.Language.Common;

namespace ASSISTENTE.Domain.Common.Interfaces;

public interface IBaseRepository<TEntity, in TIdentifier>
    where TEntity : class, IEntity, IAggregateRoot
    where TIdentifier : class, IIdentifier
{
    Task<Result<TEntity>> AddAsync(TEntity entity);
    Task<Result<TEntity>> UpdateAsync(TEntity entity);
    Task<Result> RemoveAsync(TEntity entity);
    Task<Maybe<IEnumerable<TEntity>>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<Maybe<TEntity>> GetByIdAsync(TIdentifier id);
    Task<Maybe<IEnumerable<TEntity>>> GetAllAsync();
    Task<Maybe<IEnumerable<TEntity>>> PaginateAsync(int page, int elements);
    Task<Result<int>> CountAsync();
}