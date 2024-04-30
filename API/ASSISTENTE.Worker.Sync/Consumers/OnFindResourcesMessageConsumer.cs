using ASSISTENTE.Application.Questions.Commands.FindResources;
using ASSISTENTE.Contract.Messages.Internal.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using MediatR;

namespace ASSISTENTE.Worker.Sync.Consumers;

public sealed class OnFindResourcesMessageConsumer(
    ILogger<OnFindResourcesMessageConsumer> logger, 
    ISender mediator) : ResultConsumerBase<FindResourcesMessage, FindResourcesCommand>(logger, mediator)
{
    protected override FindResourcesCommand MediatRequest(FindResourcesMessage message)
        => FindResourcesCommand.Create(message.QuestionId);
}