using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.DbSets;

public class Dish
{
    public Guid Id { get; set; }
    [MinLength(1)]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    [Url]
    public string? Image { get; set; }
    public bool Vegetarian { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public DishCategory Category { get; set; }
}