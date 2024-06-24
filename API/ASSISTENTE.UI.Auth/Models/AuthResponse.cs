namespace ASSISTENTE.UI.Auth.Models;

public sealed class AuthResponse
{
    private AuthResponse()
    {
        IsSuccess = true;
    }

    private AuthResponse(AuthError error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; set; }
    public AuthError? Error { get; set; }

    public static AuthResponse Success() => new();

    public static AuthResponse Fail(AuthError? error) => error != null
        ? new AuthResponse(error)
        : new AuthResponse(new AuthError("UnknownError", "Unknown error"));
}