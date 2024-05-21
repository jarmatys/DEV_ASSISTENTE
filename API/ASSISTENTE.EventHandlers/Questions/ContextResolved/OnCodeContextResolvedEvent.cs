using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Language.Enums;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions.ContextResolved;

public sealed class OnCodeContextResolvedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ContextResolvedEvent>
{
    public Task Handle(ContextResolvedEvent notification, CancellationToken ct)
    {
        return notification.Context == QuestionContext.Code 
            ? publishEndpoint.Publish(new FindFilesMessage(notification.QuestionId), ct) 
            : Task.CompletedTask;
    }
}