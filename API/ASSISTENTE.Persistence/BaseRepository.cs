using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language;
using ASSISTENTE.Persistence.MSSQL;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using DomainCommons = ASSISTENTE.Domain.Commons;

namespace ASSISTENTE.Persistence;

internal abstract class BaseRepository<TEntity, TIdentifier>(IAssistenteDbContext context) : IBaseRepository<TEntity, TIdentifier>
    where TEntity : DomainCommons.Entity<TIdentifier>
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

    public async Task<Maybe<TEntity>> GetByIdAsync(TIdentifier id)
    {
        var entity = await Get().SingleOrDefaultAsync(e => e.Id == id);
        
        return entity == null 
            ? Maybe<TEntity>.None 
            : Maybe<TEntity>.From(entity);
    }

    public async Task<Maybe<IEnumerable<TEntity>>> GetAllAsync()
    {
        return await Get().AsNoTracking().ToListAsync();
    }

    public async Task<Maybe<IEnumerable<TEntity>>> PaginateAsync(int page, int elements)
    {
        return await Get()
            .AsNoTracking()
            .Skip((page - 1) * elements)
            .Take(elements)
            .ToListAsync();
    }

    protected abstract IQueryable<TEntity> Get();
}