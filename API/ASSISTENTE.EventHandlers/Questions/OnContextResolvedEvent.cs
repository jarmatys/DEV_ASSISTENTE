using ASSISTENTE.Common.Messaging;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Language.Enums;
using MassTransit;
using MediatR;

namespace ASSISTENTE.EventHandlers.Questions;

public sealed class OnContextResolvedEvent(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ContextResolvedEvent>
{
    public Task Handle(ContextResolvedEvent notification, CancellationToken cancellationToken)
    {
        var message = notification.Context switch
        {
            QuestionContext.Code => new FindFilesMessage(notification.QuestionId) as IMessage,
            QuestionContext.Note => new FindResourcesMessage(notification.QuestionId) as IMessage,
            QuestionContext.Error => null,
            _ => null
        };

        if (message is not null)
            publishEndpoint.Publish(message, cancellationToken);
        
        return Task.CompletedTask;
    }
}