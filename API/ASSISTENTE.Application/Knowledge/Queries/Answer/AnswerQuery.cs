using ASSISTENTE.Application.Abstractions.Interfaces;
using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Application.Knowledge.Queries.Answer
{
    public sealed class AnswerQuery : IRequest<Result<AnswerResponse>>
    {
        private AnswerQuery(string question)
        {
            Question = question;
        }
        
        public string Question { get; }
        
        public static AnswerQuery Create(string question)
        {
            return new AnswerQuery(question);
        }
    }
    
    public class AnswerQueryHandler(IKnowledgeService knowledgeService, ILogger<AnswerQueryHandler> logger) 
        : IRequestHandler<AnswerQuery, Result<AnswerResponse>>
    {
        public async Task<Result<AnswerResponse>> Handle(AnswerQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Question: '{Question}' is being answered...", query.Question);
            
            return await knowledgeService.RecallAsync(query.Question)
                .Map(text => new AnswerResponse(text));
        }
    }
}
