using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.UI.Auth.Models.Requests;

public class LoginForm
{
    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] public string? Password { get; set; }
}
