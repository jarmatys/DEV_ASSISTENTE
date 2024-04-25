using ASSISTENTE.Language;

namespace ASSISTENTE.Domain.Commons.Interfaces;

public interface IBaseRepository<TEntity, in TIdentifier>
    where TEntity : class, IEntity
    where TIdentifier : class, IIdentifier
{
    Task<Result<TEntity>> AddAsync(TEntity entity);
    Task<Result<TEntity>> UpdateAsync(TEntity entity);
    Task<Result<TEntity>> RemoveAsync(TEntity entity);
    Task<Maybe<TEntity>> GetByIdAsync(TIdentifier id);
    Task<Maybe<IEnumerable<TEntity>>> GetAllAsync();
}