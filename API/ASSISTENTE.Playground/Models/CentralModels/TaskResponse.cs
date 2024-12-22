using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.CentralModels;

public class TaskResponse
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }
    
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}