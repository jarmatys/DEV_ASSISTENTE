using ASSISTENTE.Application.Handlers.Knowledge.Queries;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Resources;

public sealed class GetResourcesEndpoint(ISender mediator)
    : QueryEndpointBase<GetResourcesRequest, GetResourcesResponse, GetResourcesQuery>(mediator)
{
    public override void Configure()
    {
        Get("resources");
        SetupSwagger();
        AllowAnonymous();
    }

    protected override GetResourcesQuery MediatRequest(GetResourcesRequest req) 
        => GetResourcesQuery.Create(req);
}