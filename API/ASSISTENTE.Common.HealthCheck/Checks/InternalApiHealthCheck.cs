using System.Net;
using ASSISTENTE.Common.Settings;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Common.HealthCheck.Checks;

internal class InternalApiHealthCheck(AssistenteSettings settings, HttpClient httpClient) : CheckBase("InternalAPI")
{
    protected override async Task<Result> Check()
    {
        var uri = new UriBuilder(settings.InternalApi.Url) { Path = "/" }.Uri;
        
        var response = await httpClient.GetAsync(uri);
        
        return response.StatusCode == HttpStatusCode.NotFound 
            ? Result.Success() 
            : Result.Failure("Internal API is not available");
    }
}