using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Enums;

namespace ASSISTENTE.Domain.Entities.Resources;

public sealed class Resource : AuditableEntity
{
    private Resource(Guid resourceId, string content, string title, ResourceType type, IEnumerable<float> embeddings)
    {
        ResourceId = resourceId;
        Content = content;
        Title = title;
        Type = type;
        Embeddings = embeddings;
    }
    
    public Guid ResourceId { get; private set; }
    public string Content { get; private set; }
    public string Title { get; private set; }
    public ResourceType Type { get; private set; }
    public IEnumerable<float> Embeddings { get; private set; }
    
    public static Result<Resource> Create(string content, string title, ResourceType type, IEnumerable<float> embeddings)
    {
        return new Resource(Guid.NewGuid(), content, title, type, embeddings);
    }
}