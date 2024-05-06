using ASSISTENTE.Contract.Requests.Internal.Hub.NotifyQuestionReadiness;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Questions;

public sealed class NotifyQuestionReadinessRequestValidator : Validator<NotifyQuestionReadinessRequest>
{
    public NotifyQuestionReadinessRequestValidator()
    {
        RuleFor(x => x.ConnectionId)
            .NotEmpty()
            .WithMessage("'ConnectionId' field is is required!");
        
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage("'QuestionId' field is is required!");
    }
}