using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Errors;

internal static class KnowledgeServiceErrors
{
    public static readonly Error NotFound = new(
        "KnowledgeService.NotFound", "No resources found");
    
    public static readonly Error PromptTypeNotExist = new(
        "KnowledgeService.PromptTypeNotExist", "Prompt type does not exist");
    
    public static readonly Error ResourceTypeNotExist = new(
        "KnowledgeService.ResourceTypeNotExist", "Resource type does not exist");
}