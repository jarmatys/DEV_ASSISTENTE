using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;
using FastEndpoints;
using FluentValidation;

namespace ASSISTENTE.API.Validators.Questions;

public sealed class GetQuestionsRequestValidator : Validator<GetQuestionsRequest>
{
    public GetQuestionsRequestValidator()
    {
        RuleFor(x => x.Page)
            .NotEmpty()
            .WithMessage("'Page' parameter is is required!");
        
        RuleFor(x => x.Elements)
            .NotEmpty()
            .WithMessage("'Elements' parameter is is required!");
    }
}