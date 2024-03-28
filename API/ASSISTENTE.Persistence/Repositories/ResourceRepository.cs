using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Persistence.MSSQL;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.Repositories;

internal sealed class ResourceRepository(IAssistenteDbContext context) 
    : BaseRepository<Resource>(context), IResourceRepository
{
    private readonly IAssistenteDbContext _context = context;

    public async Task<Maybe<List<Resource>>> FindByResourceIdsAsync(List<Guid> resourceIds)
    {
        var resources = await _context.Resources
            .Where(x => resourceIds.Contains(x.ResourceId))
            .ToListAsync();
        
        return resources.Count == 0
            ? Maybe<List<Resource>>.None 
            : Maybe<List<Resource>>.From(resources);
    }

    protected override IQueryable<Resource> Get()
    {
        return _context.Resources
            .Include(x => x.Questions);
    }
}