using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Questions.Commands.CreateQuestion
{
    public sealed class CreateQuestionCommand : IRequest<Result>
    {
        private CreateQuestionCommand(CreateQuestionRequest request)
        {
            Question = request.Question;
            ConnectionId = request.ConnectionId;
        }
        
        public string? Question { get; }
        public string? ConnectionId { get; }
        
        public static CreateQuestionCommand Create(CreateQuestionRequest payload)
        {
            return new CreateQuestionCommand(payload);
        }
    }

    public class CreateQuestionCommandHandler(
        IKnowledgeService knowledgeService,
        ILogger<CreateQuestionCommandHandler> logger)
        : IRequestHandler<CreateQuestionCommand, Result>
    {
        public async Task<Result> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "1 | (ConnectionId: {ConnectionId}) - '{Question}' is being initiating...",
                request.ConnectionId,
                request.Question
            );
            
            return await knowledgeService.InitQuestion(request.Question!, request.ConnectionId);
        }
    }
}