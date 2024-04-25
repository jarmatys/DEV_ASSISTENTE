using FluentValidation;

namespace ASSISTENTE.Application.Knowledge.Commands.FindResources
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
