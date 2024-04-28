using FluentValidation;

namespace ASSISTENTE.Application.Questions.Commands.FindResources
{
    public class FindResourcesCommandValidator : AbstractValidator<FindResourcesCommand>
    {
        public FindResourcesCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
            
        }
    }
}
