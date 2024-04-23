using ASSISTENTE.API.Hubs;
using ASSISTENTE.Contract.Internal.Requests.Knowledge.Commands.UpdateAnswer;
using FastEndpoints;
using Microsoft.AspNetCore.SignalR;

namespace ASSISTENTE.API.Endpoints.Answers;

public sealed class PutAnswerEndpoint(IHubContext<AnswerHub, IAnswerHub> hubContext) : Endpoint<UpdateAnswerRequest> 
{
    public override void Configure()
    {
        Put("/api/answer");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateAnswerRequest req, CancellationToken ct)
    {
        // TODO: add request validation
        
        await hubContext.Clients.Client(req.ConnectionId).ReceiveAnswer("Your message here");
        
        await SendOkAsync(ct);
    }
}