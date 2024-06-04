using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Application.Handlers.Bases;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Handlers.Questions.Commands
{
    public sealed class GenerateAnswerCommand : IRequest<Result>
    {
        private GenerateAnswerCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }

        public QuestionId QuestionId { get; }

        public static GenerateAnswerCommand Create(QuestionId questionId)
        {
            return new GenerateAnswerCommand(questionId);
        }
    }
    
    public class GenerateAnswerCommandHandler(
        IQuestionOrchestrator questionOrchestrator,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<GenerateAnswerCommandHandler> logger) : QuestionCommandBase<GenerateAnswerCommand>(logger, clientInternal)
    {
        protected override async Task<Result> HandleAsync(Question question)
            => await questionOrchestrator.GenerateAnswer(question);

        protected override async Task<Maybe<Question>> GetQuestionAsync(GenerateAnswerCommand request)
            => await questionRepository.GetByIdAsync(request.QuestionId);
        
        protected override ProgressInformation InitialInformation => new(90, "Generating answer...");
        protected override ProgressInformation FinalInformation => new(100, "Answer generated!");
    }
}