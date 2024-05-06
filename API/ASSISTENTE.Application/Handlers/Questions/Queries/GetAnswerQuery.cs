using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Handlers.Questions.Queries
{
    public sealed class GetAnswerQuery : IRequest<Result<GetAnswerResponse>>
    {
        private GetAnswerQuery(GetAnswerRequest request)
        {
            QuestionId = request.QuestionId;
        }
        
        public QuestionId QuestionId { get; }
        
        public static GetAnswerQuery Create(GetAnswerRequest request)
        {
            return new GetAnswerQuery(request);
        }
    }
    
    public class GetAnswerQueryHandler(IQuestionRepository questionRepository) 
        : IRequestHandler<GetAnswerQuery, Result<GetAnswerResponse>>
    {
        public async Task<Result<GetAnswerResponse>> Handle(GetAnswerQuery query, CancellationToken cancellationToken)
        {
            return await questionRepository.GetByIdAsync(query.QuestionId)
                .ToResult(RepositoryErrors<Question>.NotFound.Build())
                .Bind(question => question.GetAnswer())
                .Map(answerText => new GetAnswerResponse(answerText));
        }
    }
}
