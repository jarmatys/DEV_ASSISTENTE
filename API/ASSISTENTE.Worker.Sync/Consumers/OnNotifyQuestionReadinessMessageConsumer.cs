using ASSISTENTE.Application.Handlers.Questions.Commands;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnNotifyQuestionReadinessMessageConsumer(
    ILogger<OnNotifyQuestionReadinessMessageConsumer> logger, 
    ISender mediator) : ResultConsumerBase<NotifyQuestionReadinessMessage, NotifyQuestionReadinessCommand>(logger, mediator)
{
    protected override NotifyQuestionReadinessCommand MediatRequest(NotifyQuestionReadinessMessage message)
        => NotifyQuestionReadinessCommand.Create(message.QuestionId);
}