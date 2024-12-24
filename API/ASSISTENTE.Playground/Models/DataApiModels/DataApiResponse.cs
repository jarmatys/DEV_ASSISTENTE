using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.DataApiModels;

public class DataApiResponse
{
    [JsonPropertyName("code")]
    public required object Code { get; set; }
    
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}