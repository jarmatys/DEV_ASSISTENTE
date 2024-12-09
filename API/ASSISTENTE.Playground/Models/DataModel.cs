using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models;

internal class DataModel
{
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [JsonPropertyName("copyright")]
    public required string Copyright { get; set; }
    
    [JsonPropertyName("apikey")] 
    public required string ApiKey { get; set; }
    
    [JsonPropertyName("test-data")]
    public required List<DataItemModel> Data { get; set; }
}

internal class DataItemModel
{
    [JsonPropertyName("question")]
    public required string  Question { get; set; }
    
    [JsonPropertyName("answer")]
    public required int Answer { get; set; }
    
    [JsonPropertyName("test")]
    public AdditionalInformationModel? AdditionalInformation { get; set; }
}

internal class AdditionalInformationModel
{
    [JsonPropertyName("q")]
    public required string Question { get; set; }
    
    [JsonPropertyName("a")]
    public required string Answer { get; set; }
}