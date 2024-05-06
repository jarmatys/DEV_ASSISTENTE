using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResource;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Resources;

public sealed class GetResourceRequestValidator : Validator<GetResourceRequest>
{
    public GetResourceRequestValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty()
            .WithMessage("'ResourceId' parameter is is required!");
    }
}