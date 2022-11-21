using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models;

public class UserEditModel
{
    [MinLength(1)]
    [JsonPropertyName("fullName")]
    public string FullName;
    [JsonPropertyName("birthDate")]
    public DateTime? BirthDate;
    [JsonPropertyName("gender")]
    public Gender Gender;
    [JsonPropertyName("address")]
    public string? Address;
    [Phone]
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber;

    public UserEditModel(string fullName, DateTime? birthDate, Gender gender, string? address, string? phoneNumber)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Gender = gender;
        Address = address;
        PhoneNumber = phoneNumber;
    }
}