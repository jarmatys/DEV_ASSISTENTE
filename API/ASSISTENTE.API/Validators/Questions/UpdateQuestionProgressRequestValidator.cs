using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Questions;

public sealed class UpdateQuestionProgressRequestValidator : Validator<UpdateQuestionProgressRequest>
{
    public UpdateQuestionProgressRequestValidator()
    {
        RuleFor(x => x.ConnectionId)
            .NotEmpty()
            .WithMessage("'ConnectionId' field is is required!");
        
        RuleFor(x => x.Progress)
            .NotEmpty()
            .WithMessage("'Progress' field is is required!");
    }
}