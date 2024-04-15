using ASSISTENTE.Application.Knowledge.Queries.Answer;
using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Answers;

public sealed class AnswerEndpoint(ISender mediator)
    : EndpointBase<AnswerRequest, AnswerResponse, AnswerQuery>(mediator)
{
    public override void Configure()
    {
        Post("/api/answer");
        AllowAnonymous();
        SetupSwagger();
    }

    protected override AnswerQuery MediatRequest(AnswerRequest req, CancellationToken ct)
        => AnswerQuery.Create(req.Question);
}