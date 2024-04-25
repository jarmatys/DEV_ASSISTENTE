using ASSISTENTE.Domain.Commons.Interfaces;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions.Interfaces;

public interface IQuestionRepository : IBaseRepository<Question, QuestionId>
{
}