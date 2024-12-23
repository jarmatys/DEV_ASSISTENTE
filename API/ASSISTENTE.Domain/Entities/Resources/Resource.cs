using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Common.Interfaces;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Resources;

public sealed class Resource : AuditableEntity<ResourceId>, IAggregateRoot
{
    private Resource()
    {
        Questions = new List<QuestionResource>();
    }
    
    private Resource(string content, string title, ResourceType type, List<float> embeddings)
    {
        Id = new ResourceId(Guid.NewGuid());
        
        Content = content;
        Title = title;
        Type = type;
        Embeddings = embeddings;
        
        Questions = new List<QuestionResource>();
    }
    
    public string Content { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public ResourceType Type { get; private set; }
    public List<float> Embeddings { get; private set; } = null!;
    
    // TODO: Save in 'Resource' table full resource content
    // Splitted chunks of content will be saved in 'ResourceChunk' table and linked to 'Resource' table
    
    // public Metadata Metadata { get; private set; } // TODO: Implement Metadata class

    #region NAVIGATION PROPERTIES

    public ICollection<QuestionResource> Questions { get; private set; }

    #endregion
    
    public static Result<Resource> Create(string content, string title, ResourceType type, List<float> embeddings)
    {
        return new Resource(content, title, type, embeddings);
    }
}