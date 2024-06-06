using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.QuestionNotes.Events;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.QuestionNotes;

public sealed class OnQuestionNoteCreatedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<QuestionNoteCreatedEvent>
{
    public Task Handle(QuestionNoteCreatedEvent notification, CancellationToken ct)
    {
        publishEndpoint.Publish(new CreateEmbeddingsMessage(notification.QuestionId), ct);

        return Task.CompletedTask;
    }
}