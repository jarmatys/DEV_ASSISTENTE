using ASSISTENTE.Domain.Commons;

namespace ASSISTENTE.Domain.Entities.Resources.Interfaces;

public interface IResourceRepository : IBaseRepository<Resource>
{
    Task<Maybe<List<Resource>>> FindByResourceIdsAsync(IEnumerable<Guid> resourceIds);
}