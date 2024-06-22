namespace ASSISTENTE.Common.Authentication.Settings;

public sealed class AuthenticationSettings
{
    public required string JwtSecret { get; init; }
    public required string ValidAudience { get; init; }
    public required string ValidIssuer { get; init; }
}