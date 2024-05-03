using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources.Models;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Knowledge.Queries.GetResources
{
    public sealed class GetResourcesQuery : IRequest<Result<GetResourcesResponse>>
    {
        public static GetResourcesQuery Create(GetResourcesRequest request)
        {
            return new GetResourcesQuery();
        }
    }
    
    public class GetResourcesQueryHandler(IResourceRepository resourceRepository) 
        : IRequestHandler<GetResourcesQuery, Result<GetResourcesResponse>>
    {
        public async Task<Result<GetResourcesResponse>> Handle(GetResourcesQuery query, CancellationToken cancellationToken)
        {
            // TODO: Implement pagination
            return await resourceRepository.GetAllAsync()
                .ToResult(RepositoryErrors<Resource>.NotFound.Build())
                .Map(resources => resources.Select(q => new ResourceDto(q.Id, q.Title, q.Type)).ToList())
                .Map(resourceDtos => new GetResourcesResponse(resourceDtos));
        }
    }
}
