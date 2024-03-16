using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Persistence.MSSQL;

namespace ASSISTENTE.Persistence.Repositories;

internal sealed class ResourceRepository(IAssistenteDbContext context) 
    : BaseRepository<Resource>(context), IResourceRepository
{
}