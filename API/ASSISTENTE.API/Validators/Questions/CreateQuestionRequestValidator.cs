using ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Questions;

public sealed class CreateQuestionRequestValidator : Validator<CreateQuestionRequest>
{
    public CreateQuestionRequestValidator()
    {
        RuleFor(x => x.Question)
            .NotEmpty()
            .WithMessage("'Question' field is is required!");

        RuleFor(x => x.ConnectionId)
            .NotEmpty()
            .WithMessage("'ConnectionId' field is is required!");
    }
}