using FluentValidation;

namespace ASSISTENTE.Application.Knowledge.Queries.GetResource
{
    public class GetQuestionQueryValidator : AbstractValidator<GetResourceQuery>
    {
        public GetQuestionQueryValidator()
        {
            RuleFor(x => x.ResourceId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
        }
    }
}
