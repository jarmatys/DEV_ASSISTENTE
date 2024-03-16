using CSharpFunctionalExtensions;

namespace ASSISTENTE.Domain.Commons;

public interface IBaseRepository<TEntity>
{
    Task<Result<TEntity>> AddAsync(TEntity entity);
    Task<Result<TEntity>> UpdateAsync(TEntity entity);
    Task<Result<TEntity>> RemoveAsync(TEntity entity);
    Task<Result<TEntity>> GetByIdAsync(int id);
    Task<Result<IEnumerable<TEntity>>> GetAllAsync();
}