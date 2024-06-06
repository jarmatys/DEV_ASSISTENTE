using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Entities.Resources;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class QuestionResource : AuditableEntity<QuestionResourceId>
{
    private QuestionResource()
    {
    }
    
    private QuestionResource(ResourceId resourceId)
    {
        ResourceId = resourceId;
    }
    
    # region NAVIGATION PROPERTIES
    
    public QuestionId QuestionId { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    public ResourceId ResourceId { get; private set; } = null!;
    public Resource Resource { get; private set; } = null!;

    # endregion
    
    internal static Result<QuestionResource> Create(ResourceId resourceId)
    {
        return new QuestionResource(resourceId);
    }
}