using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions.Models;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Questions.Queries.GetQuestions
{
    public sealed class GetQuestionsQuery : IRequest<Result<GetQuestionsResponse>>
    {
        public static GetQuestionsQuery Create(GetQuestionsRequest request)
        {
            return new GetQuestionsQuery();
        }
    }
    
    public class GetQuestionsQueryHandler(IQuestionRepository questionRepository) 
        : IRequestHandler<GetQuestionsQuery, Result<GetQuestionsResponse>>
    {
        public async Task<Result<GetQuestionsResponse>> Handle(GetQuestionsQuery query, CancellationToken cancellationToken)
        {
            // TODO: Implement pagination
            return await questionRepository.GetAllAsync()
                .ToResult(RepositoryErrors<Question>.NotFound.Build())
                .Map(questions => questions.Select(q => new QuestionDto(q.Id, q.Text)).ToList())
                .Map(questionDtos => new GetQuestionsResponse(questionDtos));
        }
    }
}
