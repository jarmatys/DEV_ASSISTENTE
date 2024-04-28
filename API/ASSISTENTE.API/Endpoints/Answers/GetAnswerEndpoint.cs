using ASSISTENTE.Application.Questions.Queries.GetAnswer;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Answers;

public sealed class GetAnswerEndpoint(ISender mediator)
    : EndpointBase<GetAnswerRequest, GetAnswerResponse, GetAnswerQuery>(mediator)
{
    public override void Configure()
    {
        Get("/api/answers");
        AllowAnonymous();
        SetupSwagger();
    }

    protected override GetAnswerQuery MediatRequest(GetAnswerRequest req, CancellationToken ct)
        => GetAnswerQuery.Create(req);
}