using ASSISTENTE.Application.Handlers.Questions.Commands;
using FluentValidation;

namespace ASSISTENTE.Application.Validators.Questions.Commands
{
    public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty()
                .WithMessage("Question text is required");
            
            RuleFor(x => x.ConnectionId)
                .NotEmpty()
                .WithMessage("ConnectionId is required");
        }
    }
}
