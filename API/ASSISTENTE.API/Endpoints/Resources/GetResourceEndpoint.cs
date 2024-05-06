using ASSISTENTE.Application.Handlers.Knowledge.Queries;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Resources;

public sealed class GetResourceEndpoint(ISender mediator)
    : QueryEndpointBase<GetResourceRequest, GetResourceResponse, GetResourceQuery>(mediator)
{
    public override void Configure()
    {
        Get("resources/{@Id}", x => new { Id = x.ResourceId });
        SetupSwagger();
        
        Summary(x =>
        {
            x.RequestParam(r => r.ResourceId, "Resource identifier");
        });
    }
    
    protected override GetResourceQuery MediatRequest(GetResourceRequest req)
        => GetResourceQuery.Create(req);
}