using FluentValidation;

namespace ASSISTENTE.Application.Knowledge.Commands.ResolveQuestionContext
{
    public class ResolveQuestionContextCommandValidator : AbstractValidator<ResolveQuestionContextCommand>
    {
        public ResolveQuestionContextCommandValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty()
                .WithMessage("Question is required");
            
            RuleFor(x => x.ConnectionId)
                .NotEmpty()
                .WithMessage("ConnectionId is required");
        }
    }
}
