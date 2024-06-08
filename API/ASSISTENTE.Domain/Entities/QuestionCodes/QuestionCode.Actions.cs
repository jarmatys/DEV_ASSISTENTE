using ASSISTENTE.Domain.Entities.QuestionCodes.Errors;

namespace ASSISTENTE.Domain.Entities.QuestionCodes;

public sealed partial class QuestionCode
{
    internal Result AddEmbeddings(IEnumerable<float> embeddings)
    {
        return PerformIfPossible(QuestionCodeActions.CreateEmbeddings, QuestionCodeStateErrors.UnableToCreateEmbeddings)
            .Tap(() => Embeddings = embeddings.ToList());
    }
    
    internal Result AddFiles()
    {
        return PerformIfPossible(QuestionCodeActions.AddFiles, QuestionCodeStateErrors.UnableToAddFiles);
    }
    
    internal Result AddResources()
    {
        return PerformIfPossible(QuestionCodeActions.AddResources, QuestionCodeStateErrors.UnableToAddResources);
    }
    
    internal Result Complete()
    {
        return PerformIfPossible(QuestionCodeActions.Complete, QuestionCodeStateErrors.UnableToComplete);
    }
}