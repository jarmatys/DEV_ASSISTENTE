using ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;

namespace ASSISTENTE.Application.Abstractions.Clients;

public interface IAssistenteClientInternal
{
    public Task UpdateQuestionAsync(UpdateQuestionRequest request);
}