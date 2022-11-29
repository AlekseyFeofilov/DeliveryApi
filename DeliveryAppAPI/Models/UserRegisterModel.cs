using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models;

public class UserRegisterModel
{
    [MinLength(1)]
    [Required]
    [JsonPropertyName("fullName")]
    public string FullName { get; }
    [MinLength(6)]
    [Required]
    [JsonPropertyName("password")]
    public string Password { get; }
    [EmailAddress]
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; }
    [JsonPropertyName("address")]
    public string? Address { get; }
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate { get; }  //todo date validation (for all project too)
    [JsonPropertyName("gender")]
    [Required]
    public Gender Gender { get; }
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; }

    public UserRegisterModel(string fullName, DateTime? birthDate, Gender gender, string? address, string email, string? phoneNumber, string password)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Gender = gender;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        Password = password;
    }
}