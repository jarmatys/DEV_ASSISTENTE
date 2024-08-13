using ASSISTENTE.Application.Handlers.Questions.Commands;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnCreateEmbeddingsMessageConsumer(
    ILogger<OnCreateEmbeddingsMessageConsumer> logger, 
    ISender mediator) : ResultConsumerBase<CreateEmbeddingsMessage, CreateEmbeddingsCommand>(logger, mediator)
{
    protected override CreateEmbeddingsCommand MediatRequest(CreateEmbeddingsMessage message)
        => CreateEmbeddingsCommand.Create(message.QuestionId);
}