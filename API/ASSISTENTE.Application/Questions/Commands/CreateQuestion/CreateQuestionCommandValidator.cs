using FluentValidation;

namespace ASSISTENTE.Application.Questions.Commands.CreateQuestion
{
    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
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
