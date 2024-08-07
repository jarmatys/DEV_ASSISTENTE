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
    public sealed class FindResourcesCommand : IRequest<Result>
    {
        private FindResourcesCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }

        public QuestionId QuestionId { get; }

        public static FindResourcesCommand Create(QuestionId questionId)
        {
            return new FindResourcesCommand(questionId);
        }
    }

    public class FindResourcesCommandHandler(
        IQuestionOrchestrator questionOrchestrator,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<FindResourcesCommandHandler> logger) : QuestionCommandBase<FindResourcesCommand>(logger, clientInternal)
    {
        protected override async Task<Result> HandleAsync(Question question)
            => await questionOrchestrator.FindResources(question);

        protected override async Task<Maybe<Question>> GetQuestionAsync(FindResourcesCommand request)
            => await questionRepository.GetByIdAsync(request.QuestionId);
        
        protected override ProgressInformation InitialInformation => new(60, "Searchin for resources..");
        protected override ProgressInformation FinalInformation => new(70, "Resources found!");
    }
}