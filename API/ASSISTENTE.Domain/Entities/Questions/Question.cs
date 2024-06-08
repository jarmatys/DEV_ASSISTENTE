using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Domain.Entities.Answers;
using ASSISTENTE.Domain.Entities.QuestionCodes;
using ASSISTENTE.Domain.Entities.QuestionNotes;
using ASSISTENTE.Domain.Entities.Questions.Enums;
using ASSISTENTE.Domain.Entities.Questions.Events;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed partial class Question : StatefulEntity<QuestionId, QuestionStates, QuestionActions>, IAggregateRoot
{
    private Question()
    {
        Resources = new List<QuestionResource>();
        Files = new List<QuestionFile>();
        
        ConfigureStateMachine();
    }

    private Question(string text, string? connectionId)
    {
        
        Id = new QuestionId(Guid.NewGuid());

        Text = text;
        ConnectionId = connectionId;
        Context = null;

        Resources = new List<QuestionResource>();
        Files = new List<QuestionFile>();
        
        RaiseEvent(new QuestionCreatedEvent(Id));
    }

    public string Text { get; private set; } = null!;
    public string? ConnectionId { get; private set; }
    public QuestionContext? Context { get; private set; }
    
    # region NAVIGATION PROPERTIES

    public Answer? Answer { get; private set; }
    public QuestionCode? CodeContext { get; private set; }
    public QuestionNote? NoteContext { get; private set; }

    public ICollection<QuestionResource> Resources { get; private set; }
    public ICollection<QuestionFile> Files { get; private set; }

    # endregion

    public static Result<Question> Create(string text, string? connectionId)
    {
        return new Question(text, connectionId);
    }
}