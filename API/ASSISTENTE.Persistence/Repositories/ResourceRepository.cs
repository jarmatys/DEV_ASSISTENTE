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

    public async Task<Maybe<Resource>> FindByResourceIdAsync(Guid resourceId)
    {
        var resource = await _context.Resources
            .FirstOrDefaultAsync(r => r.ResourceId == resourceId);
        
        return resource == null 
            ? Maybe<Resource>.None 
            : Maybe<Resource>.From(resource);
    }
}