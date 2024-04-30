using ASSISTENTE.API.Hubs;
using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;
using FastEndpoints;
using Microsoft.AspNetCore.SignalR;

namespace ASSISTENTE.API.Endpoints.Questions;

public sealed class PutQuestionProgressEndpoint(IHubContext<AnswerHub, IAnswerHub> hubContext) : Endpoint<UpdateQuestionProgressRequest> 
{
    public override void Configure()
    {
        Put("/api/questions/progress");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateQuestionProgressRequest req, CancellationToken ct)
    {
        // TODO: add request validation
        
        await hubContext.Clients.Client(req.ConnectionId).ReceiveProgress(req.Progress);
        
        await SendOkAsync(ct);
    }
}