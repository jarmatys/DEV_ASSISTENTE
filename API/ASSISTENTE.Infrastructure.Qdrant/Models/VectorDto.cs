using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Qdrant.Models;

public sealed class VectorDto : QdrantBase
{
    private VectorDto(List<float> value, string collectionName) : base(collectionName)
    {
        Value = value;
    }
    
    public List<float> Value { get; }
    
    public static Result<VectorDto> Create(List<float> value, string collectionName)
    {
        return new VectorDto(value, collectionName);
    }
}
