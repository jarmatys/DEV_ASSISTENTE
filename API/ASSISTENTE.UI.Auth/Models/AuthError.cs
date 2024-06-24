using Newtonsoft.Json;

namespace ASSISTENTE.UI.Auth.Models;

public sealed class AuthError(string error, string description)
{
    [JsonProperty("error")]
    public string? Error { get; set; } = error;

    [JsonProperty("error_description")]
    public string? Description { get; set; } = description;
}