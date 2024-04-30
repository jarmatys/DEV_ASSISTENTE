using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;
using CSharpFunctionalExtensions;

namespace ASSISTENTE.Application.Abstractions.Clients;

public interface IAssistenteClientInternal
{
    public Task<Result> UpdateQuestionProgressAsync(UpdateQuestionProgressRequest request);
}