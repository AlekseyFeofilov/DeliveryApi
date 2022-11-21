using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models;

public class LoginCredentials
{
    [JsonPropertyName("email")]
    [EmailAddress]
    [MinLength(1)]
    public string Email;
    [JsonPropertyName("password")]
    [MinLength(1)]
    public string Password;

    public LoginCredentials(string email, string password)
    {
        Email = email;
        Password = password;
    }
}