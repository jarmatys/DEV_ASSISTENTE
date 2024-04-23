using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Commons.Interfaces;

namespace ASSISTENTE.Domain.Entities.Resources.Interfaces;

public interface IResourceRepository : IBaseRepository<Resource>
{
    Task<Maybe<List<Resource>>> FindByResourceIdsAsync(IEnumerable<Guid> resourceIds);
}