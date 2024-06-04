using System.Net.Http.Json;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Contract.Requests.Internal.Hub.NotifyQuestionReadiness;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionFailed;
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
            ? await CreateError(response)
            : Result.Success();
    }

    public async Task<Result> NotifyQuestionReadinessAsync(NotifyQuestionReadinessRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"{RelativeUrl}/readiness", request);
        
        return !response.IsSuccessStatusCode 
            ? await CreateError(response)
            : Result.Success();
    }
    
    public async Task<Result> NotifyQuestionFailAsync(NotifyQuestionFailureRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"{RelativeUrl}/failure", request);
        
        return !response.IsSuccessStatusCode 
            ? await CreateError(response)
            : Result.Success();
    }
    
    private static async Task<Result> CreateError(HttpResponseMessage response)
    {
        var result = await response.Content.ReadAsStringAsync();
        
        return Result.Failure($"Internal API error ({response.StatusCode}) {Environment.NewLine} {result}");
    }
}