using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Enums;

namespace ASSISTENTE.Domain.Entities.Resources;

public sealed class Resource : AuditableEntity
{
    private Resource(Guid resourceId, string content, ResourceType type)
    {
        ResourceId = resourceId;
        Content = content;
        Type = type;
    }
    
    public Guid ResourceId { get; private set; }
    public string Content { get; private set; }
    public ResourceType Type { get; private set; }
    
    public static Resource CreateNote(string content)
    {
        return new Resource(Guid.NewGuid(), content, ResourceType.Note);
    }
    
    public static Resource CreateCode(string content)
    {
        return new Resource(Guid.NewGuid(), content, ResourceType.Note);
    }
}