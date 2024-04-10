using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Cors;

namespace ASSISTENTE.API.Endpoints;

[EnableCors(CorsConst.AllowAll)]
public abstract class EndpointBase<TReqest, TResponse>(ISender mediator) : Endpoint<TReqest, TResponse> 
    where TReqest : notnull
{
    protected readonly ISender Mediator = mediator;

    public override async Task HandleAsync(TReqest req, CancellationToken ct)
    {
        await HandleResultAsync(req, ct)
            .Tap(async result => await SendAsync(result, cancellation: ct))
            .TapError(async errorMessage =>
            {
                var error = Error.Parse(errorMessage);
                
                AddError(new ValidationFailure(error.Type, error.Description));
            
                await SendErrorsAsync(cancellation: ct);
            });
    }

    protected abstract Task<Result<TResponse>> HandleResultAsync(TReqest req, CancellationToken ct);
}