namespace ASSISTENTE.Infrastructure.Embeddings.Models;

public sealed class EmbeddingDto
{
    private EmbeddingDto(List<long> embeddings)
    {
        Embeddings = embeddings;
    }
    
    public List<long> Embeddings { get; }
    
    public static EmbeddingDto Create(List<long> embeddings)
    {
        return new EmbeddingDto(embeddings);
    }
}