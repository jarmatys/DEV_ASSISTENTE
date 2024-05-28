using System.Net.Http.Json;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.Language;
using ASSISTENTE.UI.Common.Extensions;
using ASSISTENTE.UI.Common.Models;

namespace ASSISTENTE.UI.Brokers;

public abstract partial class BrokerBase(
    IHttpClientFactory httpFactory, 
    ILogger<BrokerBase> logger, 
    string relativeUrl)
{
    private readonly HttpClient _httpClient = httpFactory.CreateClient(ApiExtensions.InternalApi);

    protected async Task<HttpResult<TResponse>> GetAsync<TResponse, TRequest>(TRequest request)
        where TRequest : GetRequestBase
    {
        return await SendRequestAsync<TResponse>(() => _httpClient.GetAsync($"{relativeUrl}?{request.QueryString()}"));
    }

    protected async Task<HttpResult<TResponse>> GetAsync<TResponse>(string path = "")
    {
        return await SendRequestAsync<TResponse>(() => _httpClient.GetAsync($"{relativeUrl}/{path}"));
    }

    protected async Task<HttpResult<TResponse>> GetDetailsAsync<TResponse, TIdentifier>(TIdentifier identifier)
        where TIdentifier : IIdentifier
    {
        return await SendRequestAsync<TResponse>(() => _httpClient.GetAsync($"{relativeUrl}/{identifier}"));
    }

    protected async Task<HttpResult> PostAsync<TRequest>(TRequest request)
        where TRequest : PostRequestBase
    {
        return await SendRequestAsync(() => _httpClient.PostAsJsonAsync(relativeUrl, request));
    }
}