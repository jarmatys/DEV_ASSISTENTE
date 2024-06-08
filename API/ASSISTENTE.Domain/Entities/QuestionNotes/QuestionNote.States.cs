using ASSISTENTE.Domain.Entities.QuestionNotes.Enums;

namespace ASSISTENTE.Domain.Entities.QuestionNotes;

public sealed partial class QuestionNote
{
    private void ConfigureStateMachine()
    {
        StateMachine.Configure(QuestionNoteStates.Created)
            .OnActivate(() => State = QuestionNoteStates.Created);
        
        StateMachine.Configure(QuestionNoteStates.Created)
            .Permit(QuestionNoteActions.CreateEmbeddings, QuestionNoteStates.EmbeddingsCreated);
        
        StateMachine.Configure(QuestionNoteStates.EmbeddingsCreated)
            .Permit(QuestionNoteActions.AddResources, QuestionNoteStates.ResourcesAdded);
                
        StateMachine.Configure(QuestionNoteStates.ResourcesAdded)
            .Permit(QuestionNoteActions.Complete, QuestionNoteStates.Completed);
    }
}

public enum QuestionNoteActions
{
    AddResources,
    CreateEmbeddings,
    Complete,
}