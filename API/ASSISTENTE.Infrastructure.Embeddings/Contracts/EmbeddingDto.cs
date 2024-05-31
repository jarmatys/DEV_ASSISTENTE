namespace ASSISTENTE.Infrastructure.Embeddings.Contracts;

public sealed class EmbeddingDto
{
    private EmbeddingDto(IEnumerable<float> embeddings)
    {
        Embeddings = embeddings;
    }

    public IEnumerable<float> Embeddings { get; }
    
    public static EmbeddingDto Create(IEnumerable<float> embeddings)
    {
        return new EmbeddingDto(embeddings);
    }
}