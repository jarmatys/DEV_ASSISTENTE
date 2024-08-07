using System.Net;
using ASSISTENTE.Client.Internal.Settings;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using SOFTURE.Common.HealthCheck.Core;

namespace ASSISTENTE.Client.Internal.HealthChecks;

internal class InternalApiHealthCheck(HttpClient httpClient, IOptions<InternalApiSettings> settings) : CheckBase
{
    protected override async Task<Result> Check()
    {
        var internalApiSettings = settings.Value;
        
        var uri = new UriBuilder(internalApiSettings.Url) { Path = "/" }.Uri;
        
        var response = await httpClient.GetAsync(uri);
        
        return response.StatusCode == HttpStatusCode.NotFound 
            ? Result.Success() 
            : Result.Failure("Internal API is not available");
    }
}