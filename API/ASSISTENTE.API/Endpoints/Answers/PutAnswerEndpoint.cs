using ASSISTENTE.Contract.Internal.Requests.Knowledge.Commands.UpdateAnswer;
using FastEndpoints;
using MediatR;

namespace ASSISTENTE.API.Endpoints.Answers;

public sealed class PutAnswerEndpoint(ISender mediator) : Endpoint<UpdateAnswerRequest, UpdateAnswerResponse> 
{
    public override void Configure()
    {
        Put("/api/answer");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateAnswerRequest req, CancellationToken ct)
    {
        // TODO: Implement endpoint reciving updates and sends to answers to proper hub connection
        // await SendAsync(result, cancellation: ct);
    }
}