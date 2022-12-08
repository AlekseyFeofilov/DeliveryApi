using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishBasketDto
{
    [JsonPropertyName("id")] 
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    [JsonPropertyName("price")]
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    [JsonPropertyName("totalPrice")]
    [Required]
    public double TotalPrice => Price * Amount;
    [JsonPropertyName("amount")]
    [Required]
    [Range(0, int.MaxValue)]
    public int Amount { get; set; }
    [JsonPropertyName("image")] 
    public string? Image { get; set; }
}