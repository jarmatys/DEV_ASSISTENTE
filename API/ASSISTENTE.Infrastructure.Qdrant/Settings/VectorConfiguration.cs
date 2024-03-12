using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Settings;

public abstract class VectorConfiguration
{
    public static VectorParams Configuration { get; } = new() { Size = 1536, Distance = Distance.Cosine, OnDisk = true };
}