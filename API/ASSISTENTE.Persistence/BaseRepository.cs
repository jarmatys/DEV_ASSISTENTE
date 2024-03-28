using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Persistence.MSSQL;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence;

internal abstract class BaseRepository<TEntity>(IAssistenteDbContext context) : IBaseRepository<TEntity>
    where TEntity : AuditableEntity
{
    public async Task<Result<TEntity>> AddAsync(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        return Result.Success(entity);
    }

    public Task<Result<TEntity>> UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TEntity>> RemoveAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<TEntity>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Maybe<IEnumerable<TEntity>>> GetAllAsync()
    {
        return await Get().ToListAsync();
    }

    protected abstract IQueryable<TEntity> Get();
}