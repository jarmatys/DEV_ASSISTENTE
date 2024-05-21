using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions;

public sealed class OnFilesAttachedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<FilesAttachedEvent>
{
    public Task Handle(FilesAttachedEvent notification, CancellationToken cancellationToken)
    {
        publishEndpoint.Publish(new CreateEmbeddingsMessage(notification.QuestionId), cancellationToken);

        return Task.CompletedTask;
    }
}