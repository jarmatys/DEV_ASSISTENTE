using ASSISTENTE.API.Extensions.Configurations;
using ASSISTENTE.Common.Results;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using CSharpFunctionalExtensions;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Cors;

namespace ASSISTENTE.API.Endpoints;

[EnableCors(CorsConst.AllowAll)]
public abstract class QueryEndpointBase<TReqest, TResponse, TMediatRequest>(ISender mediator) : Endpoint<TReqest, TResponse>
    where TReqest : GetRequestBase
    where TResponse : notnull
    where TMediatRequest : IRequest<Result<TResponse>>
{
    protected void SetupSwagger()
    {
        Description(configuration =>
        {
            configuration
                //.WithName("Get by identifier")
                .Produces<TResponse>(200, "application/json")
                .Produces<ErrorResponse>(400)
                .Produces<InternalErrorResponse>(500);
        });
    }

    public override async Task HandleAsync(TReqest req, CancellationToken ct)
    {
        var mediatRequest = MediatRequest(req);

        await mediator.Send(mediatRequest, ct)
            .Tap(async result => await SendAsync(result, cancellation: ct))
            .TapError(async errorMessage =>
            {
                var error = Error.Parse(errorMessage);

                AddError(new ValidationFailure(error.Type, error.Description));

                await SendErrorsAsync(cancellation: ct);
            });
    }

    protected abstract TMediatRequest MediatRequest(TReqest req);
}

[EnableCors(CorsConst.AllowAll)]
public abstract class CommandEndpointBase<TReqest, TMediatRequest>(ISender mediator) : Endpoint<TReqest>
    where TReqest : notnull
    where TMediatRequest : IRequest<Result>
{
    protected void SetupSwagger()
    {
        Description(b => b
            //.WithName("Get by identifier")
            .Accepts<TReqest>("application/json")
            .Produces(200)
            .Produces<ErrorResponse>(400)
            .Produces<InternalErrorResponse>(500));
    }

    public override async Task HandleAsync(TReqest req, CancellationToken ct)
    {
        var mediatRequest = MediatRequest(req);

        await mediator.Send(mediatRequest, ct)
            .Tap(async () => await SendOkAsync(ct))
            .TapError(async errorMessage =>
            {
                var error = Error.Parse(errorMessage);

                AddError(new ValidationFailure(error.Type, error.Description));

                await SendErrorsAsync(cancellation: ct);
            });
    }

    protected abstract TMediatRequest MediatRequest(TReqest req);
}
