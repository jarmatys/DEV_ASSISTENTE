using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Qdrant.Contracts;

public sealed class VectorDto : QdrantBase
{
    private VectorDto(string collectionName, IEnumerable<float> value, int elements)  : base(collectionName)
    {
        Value = value;
        Elements = elements;
    }

    private IEnumerable<float> Value { get; }
    public int Elements { get; }
    
    public static Result<VectorDto> Create(string collectionName, IEnumerable<float> value, int elements = 5)
    {
        return new VectorDto(collectionName, value, elements);
    }
    
    public float[] GetVector() => Value.ToArray();
}
