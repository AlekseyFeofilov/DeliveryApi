using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class DishDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("name"), Required, MinLength(1)]
    public string Name { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("price"), Required, Range(0, double.MaxValue)]
    public double Price { get; set; }

    [JsonPropertyName("image")] public string? Image { get; set; }
    [JsonPropertyName("vegetarian")] public bool Vegetarian { get; set; }

    [JsonPropertyName("rating"), Range(1, 10)]
    public double? Rating { get; set; }

    [JsonPropertyName("category")] public DishCategory Category { get; set; }
}