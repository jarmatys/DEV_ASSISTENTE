using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Qdrant.Contracts;

public sealed class VectorDto : QdrantBase
{
    private VectorDto(string collectionName, IEnumerable<float> value)  : base(collectionName)
    {
        Value = value;
    }

    private IEnumerable<float> Value { get; }
    
    public static Result<VectorDto> Create(string collectionName, IEnumerable<float> value)
    {
        return new VectorDto(collectionName, value);
    }
    
    public float[] GetVector() => Value.ToArray();
}
