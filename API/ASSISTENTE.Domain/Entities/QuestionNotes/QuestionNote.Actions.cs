using ASSISTENTE.Domain.Entities.QuestionNotes.Errors;

namespace ASSISTENTE.Domain.Entities.QuestionNotes;

public sealed partial class QuestionNote
{
    internal Result AddEmbeddings(IEnumerable<float> embeddings)
    {
        return PerformIfPossible(QuestionNoteActions.CreateEmbeddings, QuestionNoteStateErrors.UnableToCreateEmbeddings)
            .Tap(() => Embeddings = embeddings.ToList());
    }
    
    internal Result AddResources()
    {
        return PerformIfPossible(QuestionNoteActions.AddResources, QuestionNoteStateErrors.UnableToAddResources);
    }
    
    internal Result Complete()
    {
        return PerformIfPossible(QuestionNoteActions.Complete, QuestionNoteStateErrors.UnableToComplete);
    }
}