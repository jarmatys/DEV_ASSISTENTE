using ASSISTENTE.Application.Handlers.Questions.Queries;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class GetQuestionsEndpoint(ISender mediator)
    : QueryEndpointBase<GetQuestionsRequest, GetQuestionsResponse, GetQuestionsQuery>(mediator)
{
    public override void Configure()
    {
        Get("questions");
        SetupSwagger();
        AllowAnonymous();
    }

    protected override GetQuestionsQuery MediatRequest(GetQuestionsRequest req) 
        => GetQuestionsQuery.Create(req);
}