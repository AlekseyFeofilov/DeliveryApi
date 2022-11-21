using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id;
    [MinLength(1)]
    [JsonPropertyName("fullName")]
    public string FullName;
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate;
    [JsonPropertyName("gender")]
    public Gender Gender;
    [JsonPropertyName("address")]
    public string? Address;
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email;
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber;

    public UserDto(Guid id, string fullName, DateTime? birthDate, Gender gender, string? address, string email, string phoneNumber)
    {
        Id = id;
        FullName = fullName;
        BirthDate = birthDate;
        Gender = gender;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}