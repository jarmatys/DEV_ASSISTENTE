using ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionFailed;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Questions;

public sealed class NotifyQuestionFailureRequestValidator : Validator<NotifyQuestionFailureRequest>
{
    public NotifyQuestionFailureRequestValidator()
    {
        RuleFor(x => x.ConnectionId)
            .NotEmpty()
            .WithMessage("'ConnectionId' field is is required!");
    }
}