using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource.Models;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Handlers.Knowledge.Queries
{
    public sealed class GetResourceQuery : IRequest<Result<GetResourceResponse>>
    {
        private GetResourceQuery(GetResourceRequest request)
        {
            ResourceId = request.ResourceId;
        }
        
        
        public ResourceId ResourceId { get; }
        
        public static GetResourceQuery Create(GetResourceRequest request)
        {
            return new GetResourceQuery(request);
        }
    }
    
    public class GetResourceQueryHandler(IResourceRepository resourceRepository) 
        : IRequestHandler<GetResourceQuery, Result<GetResourceResponse>>
    {
        public async Task<Result<GetResourceResponse>> Handle(GetResourceQuery query, CancellationToken cancellationToken)
        {
            return await resourceRepository.GetByIdAsync(query.ResourceId)
                .ToResult(RepositoryErrors<Resource>.NotFound.Build())
                .Map(resource =>
                {
                    var questions = resource
                        .Questions
                        .Select(r => new QuestionDto(r.Question.Id, r.Question.Text))
                        .ToList();
                    
                    return new GetResourceResponse(
                        resource.Title, 
                        resource.Content, 
                        resource.Type, 
                        questions);
                });
        }
    }
}
