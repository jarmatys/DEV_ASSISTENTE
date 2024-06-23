using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.UI.Auth.Models.Requests;

public class RegisterForm
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Password confirmation is required.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string? Password2 { get; set; }
}
