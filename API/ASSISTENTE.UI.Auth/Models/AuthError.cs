using Newtonsoft.Json;

namespace ASSISTENTE.UI.Auth.Models;

public class AuthError
{
    [JsonProperty("error")]
    public string? Error { get; set; }

    [JsonProperty("error_description")]
    public string? Description { get; set; }
}