using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language;
using ASSISTENTE.Persistence.MSSQL;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence;

internal abstract class BaseRepository<TEntity, TIdentifier>(IAssistenteDbContext context) : IBaseRepository<TEntity, TIdentifier>
    where TEntity : class, IEntity
    where TIdentifier : class, IIdentifier
{
    public async Task<Result<TEntity>> AddAsync(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        return Result.Success(entity);
    }

    public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
        return Result.Success(entity);
    }

    public Task<Result<TEntity>> RemoveAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<TEntity>> GetByIdAsync(TIdentifier id)
    {
        throw new NotImplementedException();
    }

    public async Task<Maybe<IEnumerable<TEntity>>> GetAllAsync()
    {
        return await Get().ToListAsync();
    }

    protected abstract IQueryable<TEntity> Get();
}