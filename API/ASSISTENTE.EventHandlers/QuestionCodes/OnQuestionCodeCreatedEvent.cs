using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.QuestionCodes.Events;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.QuestionCodes;

public sealed class OnQuestionCodeCreatedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<QuestionCodeCreatedEvent>
{
    public Task Handle(QuestionCodeCreatedEvent notification, CancellationToken ct)
    {
        publishEndpoint.Publish(new FindFilesMessage(notification.QuestionId), ct);
        
        return Task.CompletedTask;
    }
}