using ASSISTENTE.Application.Handlers.Questions.Commands;
using FluentValidation;

namespace ASSISTENTE.Application.Validators.Questions.Commands
{
    public sealed class GenerateAnswerCommandValidator : AbstractValidator<GenerateAnswerCommand>
    {
        public GenerateAnswerCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
        }
    }
}
