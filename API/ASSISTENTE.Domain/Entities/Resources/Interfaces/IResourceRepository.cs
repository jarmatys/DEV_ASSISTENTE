using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Resources.Interfaces;

public interface IResourceRepository : IBaseRepository<Resource, ResourceId>
{
    Task<Maybe<List<Resource>>> FindByResourceIdsAsync(IEnumerable<ResourceId> resourceIds);
}