using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models.DataApiModels.DataModels;

public class UserConnections
{
    [JsonPropertyName("user1_id")]
    public string User1Id { get; set; }
    
    [JsonPropertyName("user2_id")]
    public string User2Id { get; set; }
}