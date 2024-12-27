using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models;

public class FineTuningModel
{
    [JsonPropertyName("messages")]
    public required List<FineTuningMessage> Messages { get; set; }
}

public class FineTuningMessage
{
    [JsonPropertyName("role")]
    public required string Role { get; set; }

    [JsonPropertyName("content")]
    public required string Content { get; set; }
}