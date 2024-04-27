using ASSISTENTE.API.Hubs;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;
using FastEndpoints;
using Microsoft.AspNetCore.SignalR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class PutQuestionEndpoint(IHubContext<AnswerHub, IAnswerHub> hubContext) : Endpoint<UpdateQuestionRequest> 
{
    public override void Configure()
    {
        Put("/api/questions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateQuestionRequest req, CancellationToken ct)
    {
        // TODO: add request validation
        
        await hubContext.Clients.Client(req.ConnectionId).ReceiveAnswer($"Status updated to {req.Progress.ToString()}");
        
        await SendOkAsync(ct);
    }
}