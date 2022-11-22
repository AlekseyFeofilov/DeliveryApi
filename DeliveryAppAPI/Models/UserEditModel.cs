using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models;

public class UserEditModel
{
    [MinLength(1)]
    [JsonPropertyName("fullName")]
    public string FullName { get; set; }
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate { get; set; }
    [JsonPropertyName("gender")]
    public Gender Gender { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }

    public UserEditModel(string fullName, DateTime? birthDate, Gender gender, string? address, string? phoneNumber)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Gender = gender;
        Address = address;
        PhoneNumber = phoneNumber;
    }
}