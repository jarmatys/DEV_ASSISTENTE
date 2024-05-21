using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Language.Enums;
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

    public async Task<Maybe<List<string>>> GetFileNames(ResourceType type)
    {
        var fileNames = await _context.Resources
            .Where(x => x.Type == type)
            .Select(x => x.Title)
            .Distinct()
            .ToListAsync();

        return fileNames.Count == 0
            ? Maybe<List<string>>.None
            : Maybe<List<string>>.From(fileNames);
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