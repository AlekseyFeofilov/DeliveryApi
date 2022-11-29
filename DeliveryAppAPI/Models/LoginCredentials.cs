using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models;

public class LoginCredentials
{
    [JsonPropertyName("email")]
    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string Email { get; }
    [JsonPropertyName("password")]
    [Required]
    [MinLength(1)]
    public string Password { get; }

    public LoginCredentials(string email, string password)
    {
        Email = email;
        Password = password;
    }
}