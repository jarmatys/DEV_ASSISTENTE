using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions;

public sealed class OnResourcesAttachedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ResourcesAttachedEvent>
{
    public Task Handle(ResourcesAttachedEvent notification, CancellationToken cancellationToken)
    {
        publishEndpoint.Publish(new GenerateAnswerMessage(notification.QuestionId), cancellationToken);

        return Task.CompletedTask;
    }
}