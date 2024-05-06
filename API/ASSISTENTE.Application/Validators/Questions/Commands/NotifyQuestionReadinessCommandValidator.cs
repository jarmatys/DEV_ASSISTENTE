using ASSISTENTE.Application.Handlers.Questions.Commands;
using FluentValidation;

namespace ASSISTENTE.Application.Validators.Questions.Commands
{
    public sealed class NotifyQuestionReadinessCommandValidator : AbstractValidator<NotifyQuestionReadinessCommand>
    {
        public NotifyQuestionReadinessCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
            
        }
    }
}
