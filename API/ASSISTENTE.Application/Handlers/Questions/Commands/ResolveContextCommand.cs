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
    public sealed class ResolveContextCommand : IRequest<Result>
    {
        private ResolveContextCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }
        
        public QuestionId QuestionId { get; }
        
        public static ResolveContextCommand Create(QuestionId questionId)
        {
            return new ResolveContextCommand(questionId);
        }
    }
    
    public class ResolveQuestionCommandHandler(
        IQuestionOrchestrator questionOrchestrator,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger logger) : QuestionCommandBase<ResolveContextCommand>(logger, clientInternal)
    {
        protected override async Task<Result> HandleAsync(Question question)
            => await questionOrchestrator.ResolveContext(question);

        protected override async Task<Maybe<Question>> GetQuestionAsync(ResolveContextCommand request)
            => await questionRepository.GetByIdAsync(request.QuestionId);
        
        protected override ProgressInformation InitialInformation => new(5, "Resolving context...");
        protected override ProgressInformation FinalInformation => new(10, "Context resolved!");
    }
}