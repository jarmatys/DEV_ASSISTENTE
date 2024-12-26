namespace ASSISTENTE.Infrastructure.Neo4J.Settings;

public sealed class Neo4JSettings
{
    public required string Url { get; init; }
    public required string User { get; init; }
    public required string Password { get; init; }
}