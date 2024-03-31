using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Errors;

internal static class KnowledgeServiceErrors
{
    public static readonly Error NotFound = new(
        "KnowledgeService.NotFound", "No resources found");
    
    public static readonly Error ContextNotRecognized = new(
        "KnowledgeService.ContextNotRecognized", "Context not recognized");
    
    public static readonly Error PromptTypeNotExist = new(
        "KnowledgeService.PromptTypeNotExist", "Prompt type does not exist");
}