using FluentValidation;

namespace ASSISTENTE.Application.Knowledge.Queries.Answer
{
    public class AnswerQueryValidator : AbstractValidator<AnswerQuery>
    {
        public AnswerQueryValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty()
                .WithMessage("Question is required");
        }
    }
}
