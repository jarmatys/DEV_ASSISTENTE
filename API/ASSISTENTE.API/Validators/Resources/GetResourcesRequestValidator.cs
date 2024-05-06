using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GetResources;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Resources;

public sealed class GetResourcesRequestValidator : Validator<GetResourcesRequest>
{
    public GetResourcesRequestValidator()
    {
        RuleFor(x => x.Page)
            .NotEmpty()
            .WithMessage("'Page' parameter is is required!");
        
        RuleFor(x => x.Elements)
            .NotEmpty()
            .WithMessage("'Page' parameter is is required!");
    }
}