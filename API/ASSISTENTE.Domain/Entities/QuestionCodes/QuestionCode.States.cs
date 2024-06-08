using ASSISTENTE.Domain.Entities.QuestionCodes.Enums;

namespace ASSISTENTE.Domain.Entities.QuestionCodes;

public sealed partial class QuestionCode
{
    private void ConfigureStateMachine()
    {
        StateMachine.Configure(QuestionCodeStates.Created)
            .OnActivate(() => State = QuestionCodeStates.Created);
        
        StateMachine.Configure(QuestionCodeStates.Created)
            .Permit(QuestionCodeActions.AddFiles, QuestionCodeStates.FilesAdded);
        
        StateMachine.Configure(QuestionCodeStates.FilesAdded)
            .Permit(QuestionCodeActions.CreateEmbeddings, QuestionCodeStates.EmbeddingsCreated);
        
        StateMachine.Configure(QuestionCodeStates.EmbeddingsCreated)
            .Permit(QuestionCodeActions.AddResources, QuestionCodeStates.ResourcesAdded);
        
        StateMachine.Configure(QuestionCodeStates.ResourcesAdded)
            .Permit(QuestionCodeActions.Complete, QuestionCodeStates.Completed);
    }
}

public enum QuestionCodeActions
{
   AddFiles,
   AddResources,
   CreateEmbeddings,
   Complete,
}