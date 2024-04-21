using FluentValidation;

namespace ASSISTENTE.Application.Knowledge.Queries.GenerateAnswer
{
    public class GenerateAnswerQueryValidator : AbstractValidator<GenerateAnswerQuery>
    {
        public GenerateAnswerQueryValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty()
                .WithMessage("Question is required");
        }
    }
}
