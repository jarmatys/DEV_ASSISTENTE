namespace ASSISTENTE.Infrastructure.Langfuse.Settings;

public sealed class LangfuseSettings
{
    public required string SecretKey { get; init; }
    public required string PublicKey { get; init; }
    public required string Host { get; init; }
}