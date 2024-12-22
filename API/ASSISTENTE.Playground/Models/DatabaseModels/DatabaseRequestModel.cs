using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.DatabaseModels;

public class DatabaseRequestModel
{
    [JsonPropertyName("task")]
    public required string Task { get; set; }
    
    [JsonPropertyName("apikey")]
    public required string ApiKey { get; set; }
    
    [JsonPropertyName("query")]
    public required string Query { get; set; }
}