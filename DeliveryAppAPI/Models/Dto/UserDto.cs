using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [MinLength(1)]
    [Required]
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("birthDate")]
    [DateRange(100 * 365, 0, true)]
    public DateTime? BirthDate { get; set; }
    [JsonPropertyName("gender")]
    [Required]
    public Gender Gender { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
}