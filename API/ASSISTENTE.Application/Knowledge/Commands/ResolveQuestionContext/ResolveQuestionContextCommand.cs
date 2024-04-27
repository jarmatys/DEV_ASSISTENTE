using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.ResolveQuestionContext;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Knowledge.Commands.ResolveQuestionContext
{
    public sealed class ResolveQuestionContextCommand : IRequest<Result>
    {
        private ResolveQuestionContextCommand(ResolveQuestionContextRequest payload)
        {
            Question = payload.Question;
            ConnectionId = payload.ConnectionId;
        }
        
        public string? Question { get; }
        public string? ConnectionId { get; }
        
        public static ResolveQuestionContextCommand Create(ResolveQuestionContextRequest payload)
        {
            return new ResolveQuestionContextCommand(payload);
        }
    }

    public class ResolveQuestionContextCommandHandler(
        IKnowledgeService knowledgeService,
        ILogger<ResolveQuestionContextCommandHandler> logger)
        : IRequestHandler<ResolveQuestionContextCommand, Result>
    {
        public async Task<Result> Handle(ResolveQuestionContextCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "1 | (ConnectionId: {ConnectionId}) - '{Question}' is being answered...",
                request.ConnectionId,
                request.Question
            );
            
            return await knowledgeService.ResolveQuestionContext(request.Question!, request.ConnectionId);
        }
    }
}