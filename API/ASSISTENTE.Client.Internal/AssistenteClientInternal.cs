using System.Net.Http.Json;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Client.Internal;

internal sealed class AssistenteClientInternal(HttpClient httpClient) : IAssistenteClientInternal
{
    private const string RelativeUrl = "api/questions";
    
    public async Task<Result> UpdateQuestionProgressAsync(UpdateQuestionProgressRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"{RelativeUrl}/progress", request);
        
        return !response.IsSuccessStatusCode 
            ? Result.Failure($"Internal API error ({response.StatusCode})") 
            : Result.Success();
    }
}