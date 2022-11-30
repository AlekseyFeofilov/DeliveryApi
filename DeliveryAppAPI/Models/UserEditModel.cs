using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models;

public class UserEditModel
{
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
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; }

    public UserEditModel(string fullName, DateTime? birthDate, Gender gender, string? address, string? phoneNumber)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Gender = gender;
        Address = address;
        PhoneNumber = phoneNumber;
    }
}