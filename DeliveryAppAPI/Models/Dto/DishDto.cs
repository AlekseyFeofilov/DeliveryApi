using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class DishDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("name")]
    [MinLength(1)]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    [JsonPropertyName("vegetarian")]
    public bool Vegetarian { get; set; }
    [JsonPropertyName("rating")]
    [Range(0, 10)]
    public double? Rating { get; set; }
    [JsonPropertyName("category")]
    public DishCategory Category { get; set; }

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