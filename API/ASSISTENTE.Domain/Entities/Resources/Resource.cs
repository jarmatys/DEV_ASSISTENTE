using ASSISTENTE.Domain.Commons;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Language.Enums;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Resources;

public sealed class Resource : AuditableEntity<ResourceId>
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

    #region NAVIGATION PROPERTIES

    public ICollection<QuestionResource> Questions { get; private set; }

    #endregion
    
    public static Result<Resource> Create(string content, string title, ResourceType type, List<float> embeddings)
    {
        return new Resource(content, title, type, embeddings);
    }
}