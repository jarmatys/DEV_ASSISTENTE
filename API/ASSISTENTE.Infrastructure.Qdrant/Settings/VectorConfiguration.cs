using Qdrant.Client.Grpc;

namespace ASSISTENTE.Infrastructure.Qdrant.Settings;

internal abstract class VectorConfiguration
{
    public static VectorParams Configuration { get; } = new() { Size = 1536, Distance = Distance.Cosine, OnDisk = true };
}