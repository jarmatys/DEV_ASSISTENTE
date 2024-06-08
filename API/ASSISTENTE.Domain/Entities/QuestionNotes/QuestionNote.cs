using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Entities.QuestionNotes.Enums;
using ASSISTENTE.Domain.Entities.QuestionNotes.Events;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.QuestionNotes;

public sealed partial class QuestionNote : StatefulEntity<QuestionNoteId, QuestionNoteStates, QuestionNoteActions>
{
    private QuestionNote()
    {
        ConfigureStateMachine();
    }

    private QuestionNote(QuestionId questionId)
    {
        Embeddings = null;
        
        RaiseEvent(new QuestionNoteCreatedEvent(questionId));
    }
    
    public List<float>? Embeddings { get; private set; }

    # region NAVIGATION PROPERTIES

    public QuestionId QuestionId { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    # endregion

    internal static Result<QuestionNote> Create(QuestionId questionId)
    {
        return new QuestionNote(questionId);
    }
}