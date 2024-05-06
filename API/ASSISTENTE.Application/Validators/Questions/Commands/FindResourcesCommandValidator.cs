using ASSISTENTE.Application.Handlers.Questions.Commands;
using FluentValidation;

namespace ASSISTENTE.Application.Validators.Questions.Commands
{
    public sealed class FindResourcesCommandValidator : AbstractValidator<FindResourcesCommand>
    {
        public FindResourcesCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
           
        }
    }
}
