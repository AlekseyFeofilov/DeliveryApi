using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.DbSets;

public class Dish
{
    [Key]
    public Guid Id;
    [MinLength(1)]
    public string Name;
    public string? Description;
    [Range(0, double.MaxValue)]
    public double Price;
    [Url]
    public string? Image;
    public bool Vegetarian;
    [Range(0, 10)]
    public double? Rating;
    public DishCategory Category;

    public Dish(Guid id, string name, string? description, double price, string? image, bool vegetarian, double? rating, DishCategory category)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Image = image;
        Vegetarian = vegetarian;
        Rating = rating;
        Category = category;
    }
}