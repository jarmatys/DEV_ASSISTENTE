using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources.Models;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Handlers.Knowledge.Queries
{
    public sealed class GetResourcesQuery : IRequest<Result<GetResourcesResponse>>
    {
        private GetResourcesQuery(GetResourcesRequest request)
        {
            Page = request.Page;
            Elements = request.Elements;
        }
        
        public int Page { get;}
        public int Elements { get; }
        
        public static GetResourcesQuery Create(GetResourcesRequest request)
        {
            return new GetResourcesQuery(request);
        }
    }
    
    public class GetResourcesQueryHandler(IResourceRepository resourceRepository) 
        : IRequestHandler<GetResourcesQuery, Result<GetResourcesResponse>>
    {
        public async Task<Result<GetResourcesResponse>> Handle(GetResourcesQuery query, CancellationToken cancellationToken)
        {
            return await resourceRepository.PaginateAsync(query.Page, query.Elements)
                .ToResult(RepositoryErrors<Resource>.NotFound.Build())
                .Map(resources => resources.Select(q => new ResourceDto(q.Id, q.Title, q.Type)).ToList())
                .Map(resourceDtos => new GetResourcesResponse(resourceDtos));
        }
    }
}
