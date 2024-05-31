using System.Linq.Expressions;
using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Language;
using ASSISTENTE.Persistence.Configuration;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

using DomainCommons = ASSISTENTE.Domain.Common;

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

    public async Task<Result> RemoveAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Maybe<IEnumerable<TEntity>>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await List()
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
        
        return Maybe<IEnumerable<TEntity>>.From(entities);
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
        return await List().AsNoTracking().ToListAsync();
    }

    public async Task<Maybe<IEnumerable<TEntity>>> PaginateAsync(int page, int elements)
    {
        return await List()
            .AsNoTracking()
            .Skip((page - 1) * elements)
            .Take(elements)
            .ToListAsync();
    }

    public async Task<Result<int>> CountAsync()
    {
        return await List().AsNoTracking().CountAsync();
    }

    protected abstract IQueryable<TEntity> Get();
    protected abstract IQueryable<TEntity> List();
}