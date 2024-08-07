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
    public sealed class FindFilesCommand : IRequest<Result>
    {
        private FindFilesCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }

        public QuestionId QuestionId { get; }

        public static FindFilesCommand Create(QuestionId questionId)
        {
            return new FindFilesCommand(questionId);
        }
    }

    public class FindFilesCommandHandler(
        IQuestionOrchestrator questionOrchestrator,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger logger) : QuestionCommandBase<FindFilesCommand>(logger, clientInternal)
    {
        protected override async Task<Result> HandleAsync(Question question)
            => await questionOrchestrator.FindFiles(question);

        protected override async Task<Maybe<Question>> GetQuestionAsync(FindFilesCommand request)
            => await questionRepository.GetByIdAsync(request.QuestionId);

        protected override ProgressInformation InitialInformation => new(30, "Finding files...");
        protected override ProgressInformation FinalInformation => new(40, "Files found!");
    }
}