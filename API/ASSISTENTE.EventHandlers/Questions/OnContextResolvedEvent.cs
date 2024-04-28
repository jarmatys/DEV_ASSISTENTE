using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions;

public sealed class OnContextResolvedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ContextResolvedEvent>
{
    public Task Handle(ContextResolvedEvent notification, CancellationToken cancellationToken)
    {
        publishEndpoint.Publish(new FindResourcesMessage(notification.QuestionId), cancellationToken);

        return Task.CompletedTask;
    }
}