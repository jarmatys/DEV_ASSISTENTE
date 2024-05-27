using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResourcesCount;
using ASSISTENTE.Language.Identifiers;
using ASSISTENTE.UI.Common.Models;

namespace ASSISTENTE.UI.Brokers.Implementations;

public interface IResourcesBroker
{
    Task<HttpResult<GetResourcesResponse>> GetResourcesAsync(GetResourcesRequest request);
    Task<HttpResult<GetResourceResponse>> GetResourceAsync(GetResourceRequest request);
    Task<HttpResult<GetResourcesCountResponse>> GetResourcesCountAsync();

}

public sealed class ResourcesBroker(HttpClient httpClient, ILogger<ResourcesBroker> logger) 
    : BrokerBase(httpClient, logger, "api/resources"), IResourcesBroker
{
    public async Task<HttpResult<GetResourcesResponse>> GetResourcesAsync(GetResourcesRequest request)
        => await GetAsync<GetResourcesResponse, GetResourcesRequest>(request);

    public async Task<HttpResult<GetResourceResponse>> GetResourceAsync(GetResourceRequest request)
        => await GetDetailsAsync<GetResourceResponse, ResourceId>(request.ResourceId);
    
    public async Task<HttpResult<GetResourcesCountResponse>> GetResourcesCountAsync()
        => await GetAsync<GetResourcesCountResponse>("count");
}