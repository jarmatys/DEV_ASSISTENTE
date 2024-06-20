namespace ASSISTENTE.Infrastructure.Qdrant.Settings;

public interface IQdrantSettings
{
    QdrantSettings Qdrant { get; init; }
}