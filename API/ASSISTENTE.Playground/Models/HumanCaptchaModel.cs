using System.Text.Json.Serialization;

namespace ASSISTENTE.Playground.Models;

public class HumanCaptchaModel
{
    [JsonPropertyName("msgID")]
    public required int MessageId { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }
}