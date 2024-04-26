using FluentValidation;

namespace ASSISTENTE.Application.Knowledge.Commands.GenerateAnswer
{
    public class GenerateAnswerCommandValidator : AbstractValidator<GenerateAnswerCommand>
    {
        public GenerateAnswerCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
            
        }
    }
}
