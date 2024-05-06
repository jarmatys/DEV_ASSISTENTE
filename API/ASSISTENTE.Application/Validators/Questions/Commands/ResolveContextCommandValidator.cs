using ASSISTENTE.Application.Handlers.Questions.Commands;
using FluentValidation;

namespace ASSISTENTE.Application.Validators.Questions.Commands
{
    public sealed class ResolveContextCommandValidator : AbstractValidator<ResolveContextCommand>
    {
        public ResolveContextCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
        }
    }
}
