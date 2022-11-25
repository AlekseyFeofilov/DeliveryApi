using System.ComponentModel.DataAnnotations;

namespace DeliveryAppAPI.Models.DbSets;

public class Review
{
    public Guid Id { get; set; }
    [Required]
    public Dish Dish { get; set; }
    public User? User { get; set; }
    [Range(1, 10)]
    public int Rating { get; set; }
}