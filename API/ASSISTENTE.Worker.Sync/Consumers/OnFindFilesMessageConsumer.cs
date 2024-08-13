using ASSISTENTE.Application.Handlers.Questions.Commands;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnFindFilesMessageConsumer(
    ILogger<OnFindFilesMessageConsumer> logger, 
    ISender mediator) : ResultConsumerBase<FindFilesMessage, FindFilesCommand>(logger, mediator)
{
    protected override FindFilesCommand MediatRequest(FindFilesMessage message)
        => FindFilesCommand.Create(message.QuestionId);
}