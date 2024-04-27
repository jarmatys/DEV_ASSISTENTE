﻿using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Application.Abstractions.Clients;
using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Knowledge.Commands.FindResources
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
        IKnowledgeService knowledgeService,
        IQuestionRepository questionRepository,
        IAssistenteClientInternal clientInternal,
        ILogger<FindResourcesCommandHandler> logger)
        : IRequestHandler<FindResourcesCommand, Result>
    {
        public async Task<Result> Handle(FindResourcesCommand request, CancellationToken cancellationToken)
        {
            return await questionRepository.GetByIdAsync(request.QuestionId)
                .ToResult(RepositoryErrors.NotFound.Build())
                .Bind(async question =>
                {
                    return await UpdateStatus(question.ConnectionId!, QuestionProgress.Started)
                        .Bind(async () =>
                        {
                            logger.LogInformation(
                                "2 | ConnectionId: ({ConnectionId}) - '{Question}' searching for resources...",
                                question.ConnectionId,
                                question.Text
                            );

                            return await knowledgeService.FindResources(question);
                        })
                        .Bind(async _ => await UpdateStatus(question.ConnectionId!, QuestionProgress.ResourcesFound));
                });
        }

        private async Task<Result> UpdateStatus(string connectionId, QuestionProgress progress)
            => await clientInternal.UpdateQuestionAsync(UpdateQuestionRequest.Create(connectionId, progress));
    }
}