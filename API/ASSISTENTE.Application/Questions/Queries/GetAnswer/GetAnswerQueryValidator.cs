using FluentValidation;

namespace ASSISTENTE.Application.Questions.Queries.GetAnswer
{
    public class GetAnswerQueryValidator : AbstractValidator<GetAnswerQuery>
    {
        public GetAnswerQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
        }
    }
}
