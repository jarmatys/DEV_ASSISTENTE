using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.DatabaseModels;

public class DatabaseResponse
{
    [JsonPropertyName("reply")]
    public required object Reply { get; set; }
    
    [JsonPropertyName("error")]
    public required string Error { get; set; }
}