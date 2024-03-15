using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Persistence.Repositories;

public sealed class ResourceRepository : IResourceRepository
{
    public Task<Result<Resource>> AddAsync(Resource entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Resource>> UpdateAsync(Resource entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Resource>> RemoveAsync(Resource entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Resource>> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Resource>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}