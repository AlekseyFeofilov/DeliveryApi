using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [MinLength(1)]
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate { get; set; }
    [JsonPropertyName("gender")]
    public Gender Gender { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    public UserDto(Guid id, string fullName, DateTime? birthDate, Gender gender, string? address, string email, string? phoneNumber)
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