using System.Net;
using ASSISTENTE.Common.HealthCheck.Core;
using ASSISTENTE.Infrastructure.Qdrant.Settings;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace ASSISTENTE.Infrastructure.Qdrant.HealthChecks;

internal class QdrantHealthCheck(HttpClient httpClient, IOptions<QdrantSettings> settings) : CheckBase
{
    protected override async Task<Result> Check()
    {
        var qdrantSettings = settings.Value;
        
        var uri = new UriBuilder(qdrantSettings.Host) { Port = qdrantSettings.ApiPort, Path = "/metrics" }.Uri;
        
        var response = await httpClient.GetAsync(uri);
        
        return response.StatusCode == HttpStatusCode.OK 
            ? Result.Success() 
            : Result.Failure("Qdrant is not available");
    }
}