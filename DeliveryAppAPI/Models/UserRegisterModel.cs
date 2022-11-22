using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models;

public class UserRegisterModel
{
    [MinLength(1)]
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate { get; set; }  //todo date validation (for all project too)
    [JsonPropertyName("gender")]
    public Gender Gender { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [EmailAddress]
    [MinLength(1)]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
    [MinLength(6)]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    public UserRegisterModel(string fullName, DateTime? birthDate, Gender gender, string? address, string? email, string? phoneNumber, string password)
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