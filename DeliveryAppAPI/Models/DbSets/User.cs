using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.DbSets;

public class User
{
    [Key]
    public Guid Id;
    [MinLength(1)]
    public string FullName;
    public DateTime? BirthDate;
    public Gender Gender;
    public string? Address;
    [EmailAddress]
    [Required]
    public string Email;
    [Phone]
    public string? PhoneNumber;

    public User(Guid id, string fullName, DateTime? birthDate, Gender gender, string? address, string email, string? phoneNumber)
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