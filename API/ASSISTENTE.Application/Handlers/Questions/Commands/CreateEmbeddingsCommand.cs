using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Application.Handlers.Bases;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Handlers.Questions.Commands
{
    public sealed class CreateEmbeddingsCommand : IRequest<Result>
    {
        private CreateEmbeddingsCommand(QuestionId questionId)
        {
            QuestionId = questionId;
        }

        public QuestionId QuestionId { get; }

        public static CreateEmbeddingsCommand Create(QuestionId questionId)
        {
            return new CreateEmbeddingsCommand(questionId);
        }
    }

    public class CreateEmbeddingsCommandHandler(
        IQuestionOrchestrator questionOrchestrator,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<CreateEmbeddingsCommand> logger) : QuestionCommandBase<CreateEmbeddingsCommand>(logger, clientInternal)
    {
        protected override async Task<Result> HandleAsync(Question question)
            => await questionOrchestrator.CreateEmbedding(question);

        protected override async Task<Maybe<Question>> GetQuestionAsync(CreateEmbeddingsCommand request)
            => await questionRepository.GetByIdAsync(request.QuestionId);

        protected override QuestionProgress InitialProgress => QuestionProgress.CreatingEmbeddings;
        protected override QuestionProgress FinalProgress => QuestionProgress.EmbeddingsCreated;
    }
}