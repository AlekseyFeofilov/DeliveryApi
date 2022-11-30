using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.DbSets;

public class User
{
    public Guid Id { get; set; }
    [MinLength(1)]
    public string FullName { get; set; }
    [DateRange(100 * 365, 0, true)]
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [MinLength(6)]
    public string Password { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<DishBasket> Cart { get; set; }
    public ICollection<Review> Reviews { get; set; }
}