using ASSISTENTE.Application.Handlers.Knowledge.Queries;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResourcesCount;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Resources;

public sealed class GetResourcesCountEndpoint(ISender mediator)
    : QueryEndpointBase<GetResourcesCountResponse, GetResourcesCountQuery>(mediator)
{
    public override void Configure()
    {
        Get("resources/count");
        SetupSwagger();
    }

    protected override GetResourcesCountQuery MediatRequest() 
        => GetResourcesCountQuery.Create();
}