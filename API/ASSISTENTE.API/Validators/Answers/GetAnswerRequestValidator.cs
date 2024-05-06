using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Answers;

public sealed class GetAnswerRequestValidator : Validator<GetAnswerRequest>
{
    public GetAnswerRequestValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage("'QuestionId' parameter is is required!");
    }
}