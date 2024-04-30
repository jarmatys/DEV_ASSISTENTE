using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;
using ASSISTENTE.Language.Enums;
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
        IQuestionOrchestrator questionOrchestrator,
        ILogger<CreateQuestionCommandHandler> logger)
        : IRequestHandler<CreateQuestionCommand, Result>
    {
        public async Task<Result> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "{StepNumber} | ConnectionId: ({ConnectionId}) - '{Question}'",
                QuestionProgress.Init,
                request.ConnectionId,
                request.Question
            );
            
            return await questionOrchestrator.InitQuestion(request.Question!, request.ConnectionId);
        }
    }
}