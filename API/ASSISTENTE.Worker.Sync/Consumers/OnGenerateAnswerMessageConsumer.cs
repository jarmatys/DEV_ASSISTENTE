using ASSISTENTE.Application.Knowledge.Commands.FindResources;
using ASSISTENTE.Contract.Internal.Messages.Knowledge;
using ASSISTENTE.Worker.Sync.Common.Bases;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Worker.Sync.Consumers;

// public sealed class OnGenerateAnswerMessageConsumer (ISender mediator) 
//     : ResultConsumerBase<GenerateAnswerMessage, ...>(mediator)
// {
//     
// }