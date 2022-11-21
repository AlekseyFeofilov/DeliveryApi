using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class DishDto
{
    [JsonPropertyName("id")]
    public Guid Id;
    [JsonPropertyName("name")]
    [MinLength(1)]
    public string Name;
    [JsonPropertyName("description")]
    public string? Description;
    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public double Price;
    [JsonPropertyName("image")]
    public string? Image;
    [JsonPropertyName("vegetarian")]
    public bool Vegetarian;
    [JsonPropertyName("rating")]
    [Range(0, 10)]
    public double? Rating;
    [JsonPropertyName("category")]
    public DishCategory Category;

    public DishDto(Guid id, string name, string? description, double price, string? image, bool vegetarian, double? rating, DishCategory category)
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