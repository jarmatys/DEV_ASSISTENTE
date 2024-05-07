using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResourcesCount;
using ASSISTENTE.Domain.Entities.Resources.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Handlers.Knowledge.Queries
{
    public sealed class GetResourcesCountQuery : IRequest<Result<GetResourcesCountResponse>>
    {
        public static GetResourcesCountQuery Create()
        {
            return new GetResourcesCountQuery();
        }
    }

    public class GetResourcesCountQueryHandler(IResourceRepository resourceRepository)
        : IRequestHandler<GetResourcesCountQuery, Result<GetResourcesCountResponse>>
    {
        public async Task<Result<GetResourcesCountResponse>> Handle(
            GetResourcesCountQuery query,
            CancellationToken cancellationToken)
        {
            return await resourceRepository.CountAsync()
                .Map(count => new GetResourcesCountResponse(count));
        }
    }
}