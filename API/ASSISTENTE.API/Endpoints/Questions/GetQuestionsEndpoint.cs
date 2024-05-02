using ASSISTENTE.Application.Questions.Queries.GetQuestions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class GetQuestionsEndpoint(ISender mediator)
    : QueryEndpointBase<GetQuestionsRequest, GetQuestionsResponse, GetQuestionsQuery>(mediator)
{
    public override void Configure()
    {
        Get("/api/questions");
        AllowAnonymous();
        SetupSwagger();
    }

    protected override GetQuestionsQuery MediatRequest(GetQuestionsRequest req) 
        => GetQuestionsQuery.Create(req);
}