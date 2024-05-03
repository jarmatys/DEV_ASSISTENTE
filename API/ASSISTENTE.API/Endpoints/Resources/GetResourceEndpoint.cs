using ASSISTENTE.Application.Knowledge.Queries.GetResource;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Resources;

public sealed class GetResourceEndpoint(ISender mediator)
    : QueryEndpointBase<GetResourceRequest, GetResourceResponse, GetResourceQuery>(mediator)
{
    public override void Configure()
    {
        Get("/api/resources/{@Id}", x => new { Id = x.ResourceId });
        AllowAnonymous();
        SetupSwagger();
    }
    
    protected override GetResourceQuery MediatRequest(GetResourceRequest req)
        => GetResourceQuery.Create(req);
}