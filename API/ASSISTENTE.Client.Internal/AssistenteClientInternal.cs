using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;

namespace ASSISTENTE.Client.Internal;

internal sealed class AssistenteClientInternal : IAssistenteClientInternal
{
    public Task UpdateQuestionAsync(UpdateQuestionRequest request)
    {
        throw new NotImplementedException();
    }
}