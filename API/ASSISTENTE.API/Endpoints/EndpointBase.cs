using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Cors;

namespace ASSISTENTE.API.Endpoints;

[EnableCors(CorsConst.AllowAll)]
public abstract class EndpointBase<TReqest, TResponse, TMediatRequest>(ISender mediator) : Endpoint<TReqest, TResponse> 
    where TReqest : notnull
    where TResponse : notnull
    where TMediatRequest : IRequest<Result<TResponse>>
{
    public override async Task HandleAsync(TReqest req, CancellationToken ct)
    {
        var mediatRequest = MediatRequest(req, ct);
        
        await mediator.Send(mediatRequest, ct)
            .Tap(async result => await SendAsync(result, cancellation: ct))
            .TapError(async errorMessage =>
            {
                var error = Error.Parse(errorMessage);
                
                AddError(new ValidationFailure(error.Type, error.Description));
            
                await SendErrorsAsync(cancellation: ct);
            });
    }

    protected abstract TMediatRequest MediatRequest(TReqest req, CancellationToken ct);
}