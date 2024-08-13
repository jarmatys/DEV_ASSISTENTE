using ASSISTENTE.Application.Handlers.Questions.Commands;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnGenerateAnswerMessageConsumer(
    ILogger<OnGenerateAnswerMessageConsumer> logger, 
    ISender mediator) : ResultConsumerBase<GenerateAnswerMessage, GenerateAnswerCommand>(logger, mediator)
{
    protected override GenerateAnswerCommand MediatRequest(GenerateAnswerMessage message)
        => GenerateAnswerCommand.Create(message.QuestionId);
}