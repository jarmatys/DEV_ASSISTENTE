using System.Diagnostics;
using System.Net.Http.Json;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.Language;
using ASSISTENTE.UI.Common.Models;
using Serilog.Context;

namespace ASSISTENTE.UI.Brokers;

public abstract partial class BrokerBase(HttpClient httpClient, ILogger<BrokerBase> logger, string relativeUrl)
{
    protected async Task<HttpResult<TResponse>> GetAsync<TResponse, TRequest>(TRequest request)
        where TRequest : GetRequestBase
    {
        return await SendRequestAsync<TResponse>(() => httpClient.GetAsync($"{relativeUrl}?{request.QueryString()}"));
    }

    protected async Task<HttpResult<TResponse>> GetAsync<TResponse>(string path = "")
    {
        return await SendRequestAsync<TResponse>(() => httpClient.GetAsync($"{relativeUrl}/{path}"));
    }

    protected async Task<HttpResult<TResponse>> GetDetailsAsync<TResponse, TIdentifier>(TIdentifier identifier)
        where TIdentifier : IIdentifier
    {
        return await SendRequestAsync<TResponse>(() => httpClient.GetAsync($"{relativeUrl}/{identifier}"));
    }

    protected async Task<HttpResult> PostAsync<TRequest>(TRequest request)
        where TRequest : PostRequestBase
    {
        return await SendRequestAsync(() => httpClient.PostAsJsonAsync(relativeUrl, request));
    }
}