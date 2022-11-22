using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishBasketDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
    [MinLength(1)]
    public string Name { get; set; }
    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    [JsonPropertyName("totalPrice")]
    public double TotalPrice => Price * Amount;
    [JsonPropertyName("amount")]
    [Range(0, int.MaxValue)]
    public int Amount { get; set; }
    [JsonPropertyName("image")]
    public string? Image { get; set; }
    
    
    public DishBasketDto(Guid id, string name, double price, int amount, string? image)
    {
        Id = id;
        Name = name;
        Price = price;
        Amount = amount;
        Image = image;
    }
}