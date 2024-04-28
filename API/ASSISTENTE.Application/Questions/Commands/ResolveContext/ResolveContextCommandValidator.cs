using FluentValidation;

namespace ASSISTENTE.Application.Questions.Commands.ResolveContext
{
    public class ResolveContextCommandValidator : AbstractValidator<ResolveContextCommand>
    {
        public ResolveContextCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
        }
    }
}
