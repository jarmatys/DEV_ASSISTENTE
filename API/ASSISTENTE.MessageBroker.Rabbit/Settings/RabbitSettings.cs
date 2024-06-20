namespace ASSISTENTE.MessageBroker.Rabbit.Settings;

public sealed class RabbitSettings
{
    public required string Name { get; init; }
    public required string Url { get; init; }
}