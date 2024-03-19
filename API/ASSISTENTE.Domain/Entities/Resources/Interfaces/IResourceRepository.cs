using ASSISTENTE.Domain.Commons;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Domain.Entities.Resources.Interfaces;

public interface IResourceRepository : IBaseRepository<Resource>
{
    Task<Maybe<List<Resource>>> FindByResourceIdsAsync(List<Guid> resourceIds);
}