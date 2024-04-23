namespace ASSISTENTE.Domain.Commons.Interfaces;

public interface IBaseRepository<TEntity>
{
    Task<Result<TEntity>> AddAsync(TEntity entity);
    Task<Result<TEntity>> UpdateAsync(TEntity entity);
    Task<Result<TEntity>> RemoveAsync(TEntity entity);
    Task<Maybe<TEntity>> GetByIdAsync(int id);
    Task<Maybe<IEnumerable<TEntity>>> GetAllAsync();
}