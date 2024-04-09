using ASSISTENTE.API.Extensions;
using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;
using FastEndpoints;
using Microsoft.AspNetCore.Cors;

namespace ASSISTENTE.API.Endpoints.Answers;

[EnableCors(CorsConst.AllowAll)]
public sealed class AnswerEndpoint : Endpoint<AnswerRequest, AnswerResponse>
{
    public override void Configure()
    {
        Post("/api/answer");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AnswerRequest req, CancellationToken ct)
    {
        await SendAsync(new AnswerResponse("Hello World!"), cancellation: ct);
    }
}