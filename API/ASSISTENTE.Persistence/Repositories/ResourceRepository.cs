using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Language.Identifiers;
using ASSISTENTE.Persistence.Configuration;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.Repositories;

internal sealed class ResourceRepository(IAssistenteDbContext context)
    : BaseRepository<Resource, ResourceId>(context), IResourceRepository
{
    private readonly IAssistenteDbContext _context = context;

    public async Task<Maybe<List<Resource>>> FindByResourceIdsAsync(IEnumerable<ResourceId> resourceIds)
    {
        var resources = await _context.Resources
            .Where(r => resourceIds.Contains(r.Id))
            .ToListAsync();
        
        return resources.Count == 0
            ? Maybe<List<Resource>>.None
            : Maybe<List<Resource>>.From(resources);
    }

    protected override IQueryable<Resource> Get()
    {
        return _context.Resources
            .Include(x => x.Questions)
            .ThenInclude(x => x.Question);
    }

    protected override IQueryable<Resource> List()
    {
        return Get()
            .OrderByDescending(x => x.Created);
    }
}