using System.Net;
using ASSISTENTE.Common.Settings;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Common.HealthCheck.Checks;

internal class QdrantHealthCheck(HttpClient httpClient, AssistenteSettings settings) : CheckBase("Qdrant")
{
    protected override async Task<Result> Check()
    {
        var uri = new UriBuilder(settings.Qdrant.Host) { Port = settings.Qdrant.ApiPort, Path = "/metrics" }.Uri;
        
        var response = await httpClient.GetAsync(uri);
        
        return response.StatusCode == HttpStatusCode.OK 
            ? Result.Success() 
            : Result.Failure("Qdrant is not available");
    }
}