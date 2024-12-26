using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.DataApiModels.DataModels;

public class UserDetails
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    [JsonPropertyName("access_level")]
    public string AccessLevel { get; set; }
    
    [JsonPropertyName("is_active")]
    public string IsActive { get; set; }
    
    [JsonPropertyName("lastlog")]
    public string Lastlog { get; set; }
}