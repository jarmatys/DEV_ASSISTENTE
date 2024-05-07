using ASSISTENTE.Application.Abstractions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions.Models;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Handlers.Questions.Queries
{
    public sealed class GetQuestionsQuery : IRequest<Result<GetQuestionsResponse>>
    {
        private GetQuestionsQuery(GetQuestionsRequest request)
        {
            Page = request.Page;
            Elements = request.Elements;
        }
        
        public int Page { get;}
        public int Elements { get; }

        public static GetQuestionsQuery Create(GetQuestionsRequest request)
        {
            return new GetQuestionsQuery(request);
        }
    }

    public class GetQuestionsQueryHandler(IQuestionRepository questionRepository)
        : IRequestHandler<GetQuestionsQuery, Result<GetQuestionsResponse>>
    {
        public async Task<Result<GetQuestionsResponse>> Handle(
            GetQuestionsQuery query,
            CancellationToken cancellationToken)
        {
            return await questionRepository.PaginateAsync(query.Page, query.Elements)
                .ToResult(RepositoryErrors<Question>.NotFound.Build())
                .Map(questions => questions.Select(q => new QuestionDto(q.Id, q.Text)).ToList())
                .Map(questionDtos => new GetQuestionsResponse(questionDtos));
        }
    }
}