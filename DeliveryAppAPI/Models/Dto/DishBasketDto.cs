using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishBasketDto
{
    [JsonPropertyName("id")] 
    public Guid Id { get; }
    [JsonPropertyName("name")]
    [Required]
    [MinLength(1)]
    public string Name { get; }
    [JsonPropertyName("price")]
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; }
    [JsonPropertyName("totalPrice")]
    [Required]
    public double TotalPrice => Price * Amount;
    [JsonPropertyName("amount")]
    [Required]
    [Range(0, int.MaxValue)]
    public int Amount { get; }
    [JsonPropertyName("image")] 
    public string? Image { get; }
    
    
    public DishBasketDto(Guid id, string name, double price, int amount, string? image)
    {
        Id = id;
        Name = name;
        Price = price;
        Amount = amount;
        Image = image;
    }
}