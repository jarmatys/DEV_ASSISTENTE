using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestionsCount;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using CSharpFunctionalExtensions;
using MediatR;

namespace ASSISTENTE.Application.Handlers.Questions.Queries
{
    public sealed class GetQuestionsCountQuery : IRequest<Result<GetQuestionsCountResponse>>
    {
        public static GetQuestionsCountQuery Create()
        {
            return new GetQuestionsCountQuery();
        }
    }

    public class GetQuestionsCountQueryHandler(IQuestionRepository questionRepository)
        : IRequestHandler<GetQuestionsCountQuery, Result<GetQuestionsCountResponse>>
    {
        public async Task<Result<GetQuestionsCountResponse>> Handle(
            GetQuestionsCountQuery query,
            CancellationToken cancellationToken)
        {
            return await questionRepository.CountAsync()
                .Map(count => new GetQuestionsCountResponse(count));
        }
    }
}