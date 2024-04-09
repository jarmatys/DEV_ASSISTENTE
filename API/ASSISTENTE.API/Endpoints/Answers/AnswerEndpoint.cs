using ASSISTENTE.API.Extensions;
using ASSISTENTE.Application.Knowledge.Queries.Answer;
using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Cors;

namespace ASSISTENTE.API.Endpoints.Answers;

[EnableCors(CorsConst.AllowAll)]
public sealed class AnswerEndpoint(ISender mediator) : Endpoint<AnswerRequest, AnswerResponse>
{
    public override void Configure()
    {
        Post("/api/answer");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AnswerRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(AnswerQuery.Create(req.Question), ct);

        // TODO: Create base logic to handle the result
        
        await SendAsync(new AnswerResponse("Hello World!"), cancellation: ct);
    }
}