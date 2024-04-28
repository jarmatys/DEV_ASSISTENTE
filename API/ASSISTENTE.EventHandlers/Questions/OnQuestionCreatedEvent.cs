using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions;

public sealed class OnQuestionCreatedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<QuestionCreatedEvent>
{
    public Task Handle(QuestionCreatedEvent notification, CancellationToken cancellationToken)
    {
        publishEndpoint.Publish(new ResolveContextMessage(notification.QuestionId), cancellationToken);

        return Task.CompletedTask;
    }
}