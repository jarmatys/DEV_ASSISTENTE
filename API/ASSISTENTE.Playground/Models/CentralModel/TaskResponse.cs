using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.CentralModel;

public class TaskResponse
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }
    
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}