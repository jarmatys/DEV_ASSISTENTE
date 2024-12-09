using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models;

public class TaskRequestModel
{
    [JsonPropertyName("task")]
    public required string Task { get; set; }
    
    [JsonPropertyName("apikey")]
    public required string ApiKey { get; set; }
    
    [JsonPropertyName("answer")]
    public required object Answer { get; set; }
}