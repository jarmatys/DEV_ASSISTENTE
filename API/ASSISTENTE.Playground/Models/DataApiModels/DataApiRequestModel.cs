using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.DataApiModels;

public class DataApiRequestModel
{
    [JsonPropertyName("apikey")]
    public required string ApiKey { get; set; }
    
    [JsonPropertyName("query")]
    public required string Query { get; set; }
}