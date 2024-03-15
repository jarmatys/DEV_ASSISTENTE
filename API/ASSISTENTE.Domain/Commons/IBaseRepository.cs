using CSharpFunctionalExtensions;

namespace ASSISTENTE.Domain.Commons;

public interface IBaseRepository<T>
{
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result<T>> RemoveAsync(T entity);
    Task<Result<T>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<T>>> GetAllAsync();
}