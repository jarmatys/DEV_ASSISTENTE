using System.Net.Http.Json;
using ASSISTENTE.Contract.Common.RequestBases;
using ASSISTENTE.Language.Common;
using ASSISTENTE.UI.Brokers.Models;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.UI.Brokers;

public abstract partial class BrokerBase(
    IHttpClientFactory httpFactory, 
    ILogger<BrokerBase> logger, 
    string relativeUrl,
    string clientName)
{
    private readonly HttpClient _httpClient = httpFactory.CreateClient(clientName);

    protected async Task<HttpResult<TResponse>> GetAsync<TResponse, TRequest>(TRequest request)
        where TRequest : GetRequestBase
        where TResponse : class
    {
        return await SendRequestAsync<TResponse>(() => _httpClient.GetAsync($"{relativeUrl}?{request.QueryString()}"));
    }

    protected async Task<HttpResult<TResponse>> GetAsync<TResponse>(string path = "")
        where TResponse : class
    {
        return await SendRequestAsync<TResponse>(() => _httpClient.GetAsync($"{relativeUrl}/{path}"));
    }

    protected async Task<HttpResult<TResponse>> GetDetailsAsync<TResponse, TIdentifier>(TIdentifier identifier)
        where TResponse : class
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