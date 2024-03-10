namespace ASSISTENTE.Infrastructure.Embeddings.Models;

public sealed class EmbeddingDto
{
    private EmbeddingDto(List<double> embeddings)
    {
        Embeddings = embeddings;
    }
    
    public List<double> Embeddings { get; }
    
    public static EmbeddingDto Create(List<double> embeddings)
    {
        return new EmbeddingDto(embeddings);
    }
}