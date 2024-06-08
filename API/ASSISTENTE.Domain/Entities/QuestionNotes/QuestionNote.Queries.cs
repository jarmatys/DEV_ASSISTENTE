using ASSISTENTE.Domain.Entities.Questions.Errors;

namespace ASSISTENTE.Domain.Entities.QuestionNotes;

public sealed partial class QuestionNote
{
    public Result<List<float>> GetEmbeddings()
    {
        return Embeddings ?? Result.Failure<List<float>>(QuestionErrors.EmbeddingsNotCreated.Build());
    }
}