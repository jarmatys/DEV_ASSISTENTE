namespace ASSISTENTE.Infrastructure.LLM.OpenAi.Settings;

public sealed class OpenAiSettings
{
    public required string ApiKey { get; init; }
    public required string OrganizationId { get; init; }
    public required string ProjectId { get; init; }
}