using ASSISTENTE.Common.Results;

namespace ASSISTENTE.Infrastructure.Errors;

internal static class KnowledgeServiceErrors
{
    public static readonly Error NotFound = new(
        "KnowledgeService.NotFound", "No resources found");
}