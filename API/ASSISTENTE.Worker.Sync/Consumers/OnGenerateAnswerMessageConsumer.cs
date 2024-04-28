using ASSISTENTE.Application.Questions.Commands.GenerateAnswer;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnGenerateAnswerMessageConsumer(ISender mediator)
    : ResultConsumerBase<GenerateAnswerMessage, GenerateAnswerCommand>(mediator)
{
    protected override GenerateAnswerCommand MediatRequest(GenerateAnswerMessage message)
        => GenerateAnswerCommand.Create(message.QuestionId);
}