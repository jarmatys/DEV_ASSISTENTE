using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Entities.QuestionCodes.Enums;
using ASSISTENTE.Domain.Entities.QuestionCodes.Events;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.QuestionCodes;

public sealed partial class QuestionCode : StatefulEntity<QuestionCodeId, QuestionCodeStates, QuestionCodeActions>
{
    private QuestionCode()
    {
        ConfigureStateMachine();
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
}