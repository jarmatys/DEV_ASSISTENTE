using FluentValidation;

namespace ASSISTENTE.Application.Questions.Queries.GetQuestion
{
    public class GetQuestionQueryValidator : AbstractValidator<GetQuestionQuery>
    {
        public GetQuestionQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required");
        }
    }
}
