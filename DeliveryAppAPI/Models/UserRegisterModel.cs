using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models;

public class UserRegisterModel
{
    [MinLength(1)]
    [JsonPropertyName("fullName")]
    public string FullName;
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate;  //todo date validation (for all project too)
    [JsonPropertyName("gender")]
    public Gender Gender;
    [JsonPropertyName("address")]
    public string? Address;
    [EmailAddress]
    [MinLength(1)]
    [JsonPropertyName("email")]
    public string? Email;
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber;
    [MinLength(6)]
    [JsonPropertyName("password")]
    public string Password;

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