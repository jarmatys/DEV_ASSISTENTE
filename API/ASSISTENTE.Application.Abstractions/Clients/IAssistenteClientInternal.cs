using ASSISTENTE.Contract.Requests.Internal.Hub.NotifyQuestionReadiness;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionFailed;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Clients;

public interface IAssistenteClientInternal
{
    public Task<Result> UpdateQuestionProgressAsync(UpdateQuestionProgressRequest request);
    public Task<Result> NotifyQuestionReadinessAsync(NotifyQuestionReadinessRequest request);
    public Task<Result> NotifyQuestionFailAsync(NotifyQuestionFailureRequest request);
}