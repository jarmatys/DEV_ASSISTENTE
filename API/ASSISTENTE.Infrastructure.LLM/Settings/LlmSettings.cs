namespace ASSISTENTE.Infrastructure.LLM.Settings;

public sealed class LlmSettings
{
    public required string ApiKey { get; init; }
    public required string OrganizationId { get; init; }
    public required string ProjectId { get; init; }
}