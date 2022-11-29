using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class DishDto
{
    [JsonPropertyName("id")] public Guid Id { get; }

    [JsonPropertyName("name"), Required, MinLength(1)]
    public string Name { get; }

    [JsonPropertyName("description")] public string? Description { get; }

    [JsonPropertyName("price"), Required, Range(0, double.MaxValue)]
    public double Price { get; }

    [JsonPropertyName("image")] public string? Image { get; }
    [JsonPropertyName("vegetarian")] public bool Vegetarian { get; }

    [JsonPropertyName("rating"), Range(0, 10)]
    public double? Rating { get; }

    [JsonPropertyName("category")] public DishCategory Category { get; }

    public DishDto(Guid id, string name, string? description, double price, string? image, bool vegetarian,
        double? rating, DishCategory category)
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