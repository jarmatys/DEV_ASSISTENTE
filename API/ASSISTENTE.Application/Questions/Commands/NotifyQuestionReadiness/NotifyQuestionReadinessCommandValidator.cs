using FluentValidation;

namespace ASSISTENTE.Application.Questions.Commands.NotifyQuestionReadiness
{
    public class NotifyQuestionReadinessCommandValidator : AbstractValidator<NotifyQuestionReadinessCommand>
    {
        public NotifyQuestionReadinessCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
            
        }
    }
}
