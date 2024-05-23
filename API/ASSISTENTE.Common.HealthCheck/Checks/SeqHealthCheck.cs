using System.Net;
using ASSISTENTE.Common.Settings;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Common.HealthCheck.Checks;

internal class SeqHealthCheck(AssistenteSettings settings, HttpClient httpClient) : CheckBase("Seq")
{
    protected override async Task<Result> Check()
    {
        var uri = new UriBuilder(settings.Seq.Url) { Path = "/" }.Uri;
        
        var response = await httpClient.GetAsync(uri);
        
        return response.StatusCode == HttpStatusCode.NotFound 
            ? Result.Success() 
            : Result.Failure("Seq is not available");
    }
}