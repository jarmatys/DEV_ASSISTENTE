using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Language.Enums;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions.ContextResolved;

public sealed class OnNoteContextResolvedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ContextResolvedEvent>
{
    public Task Handle(ContextResolvedEvent notification, CancellationToken ct)
    {
        return notification.Context == QuestionContext.Note 
            ? publishEndpoint.Publish(new FindResourcesMessage(notification.QuestionId), ct) 
            : Task.CompletedTask;
    }
}