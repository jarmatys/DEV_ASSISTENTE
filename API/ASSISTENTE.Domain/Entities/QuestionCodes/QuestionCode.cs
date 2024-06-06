using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Entities.QuestionCodes.Events;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Errors;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.QuestionCodes;

public sealed class QuestionCode : AuditableEntity<QuestionCodeId>
{
    private QuestionCode()
    {
    }

    private QuestionCode(QuestionId questionId)
    {
        Embeddings = null;
        
        RaiseEvent(new QuestionCodeCreatedEvent(questionId));
    }
    
    public List<float>? Embeddings { get; private set; }

    # region NAVIGATION PROPERTIES

    public QuestionId QuestionId { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    # endregion

    internal static Result<QuestionCode> Create(QuestionId questionId)
    {
        return new QuestionCode(questionId);
    }
    
    internal Result AddEmbeddings(IEnumerable<float> embeddings)
    {
        Embeddings = embeddings.ToList();
        
        return Result.Success();
    }
    
    public Result<List<float>> GetEmbeddings()
    {
        return Embeddings ?? Result.Failure<List<float>>(QuestionErrors.EmbeddingsNotCreated.Build());
    }
}