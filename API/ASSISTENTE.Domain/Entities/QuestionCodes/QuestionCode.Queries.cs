using ASSISTENTE.Domain.Entities.Questions.Errors;

namespace ASSISTENTE.Domain.Entities.QuestionCodes;

public sealed partial class QuestionCode
{
    public Result<List<float>> GetEmbeddings()
    {
        return Embeddings ?? Result.Failure<List<float>>(QuestionErrors.EmbeddingsNotCreated.Build());
    }
}