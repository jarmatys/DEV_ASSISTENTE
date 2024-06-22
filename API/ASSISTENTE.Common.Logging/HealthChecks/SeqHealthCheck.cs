using System.Net;
using ASSISTENTE.Common.HealthCheck.Core;
using ASSISTENTE.Common.Logging.Settings;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;

namespace ASSISTENTE.Common.Logging.HealthChecks;

internal sealed class SeqHealthCheck(HttpClient httpClient, IOptions<SeqSettings> settings) : CheckBase
{
    protected override async Task<Result> Check()
    {
        var seqSettings = settings.Value;
        
        var uri = new UriBuilder(seqSettings.Url) { Path = "/" }.Uri;
        
        var response = await httpClient.GetAsync(uri);
        
        return response.StatusCode == HttpStatusCode.NotFound 
            ? Result.Success() 
            : Result.Failure("Seq is not available");
    }
}