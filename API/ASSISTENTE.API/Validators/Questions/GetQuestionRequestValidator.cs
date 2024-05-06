using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Questions;

public sealed class GetQuestionRequestValidator : Validator<GetQuestionRequest>
{
    public GetQuestionRequestValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage("'QuestionId' parameter is is required!");
    }
}