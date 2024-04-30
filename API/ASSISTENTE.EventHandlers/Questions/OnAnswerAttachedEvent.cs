using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions;

public sealed class OnAnswerAttachedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<AnswerAttachedEvent>
{
    public Task Handle(AnswerAttachedEvent notification, CancellationToken cancellationToken)
    {
        publishEndpoint.Publish(new NotifyQuestionReadinessMessage(notification.QuestionId), cancellationToken);

        return Task.CompletedTask;
    }
}