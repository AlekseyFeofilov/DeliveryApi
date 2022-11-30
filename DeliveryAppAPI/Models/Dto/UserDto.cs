using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; }
    [MinLength(1)]
    [Required]
    [JsonPropertyName("fullName")]
    public string FullName { get; }
    [JsonPropertyName("birthDate")]
    [DateRange(100 * 365, 0, true)]
    public DateTime? BirthDate { get; }
    [JsonPropertyName("gender")]
    [Required]
    public Gender Gender { get; }
    [JsonPropertyName("address")]
    public string? Address { get; }
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; }
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; }

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