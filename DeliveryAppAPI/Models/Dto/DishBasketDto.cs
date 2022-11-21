using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class DishBasketDto
{
    [JsonPropertyName("id")]
    public Guid Id;
    [JsonPropertyName("name")]
    [MinLength(1)]
    public string Name;
    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public double Price;
    [JsonPropertyName("totalPrice")]
    public double TotalPrice => Price * Amount;
    [JsonPropertyName("amount")]
    [Range(0, int.MaxValue)]
    public int Amount;
    [JsonPropertyName("image")]
    public string? Image;
    
    
    public DishBasketDto(Guid id, string name, double price, int amount, string? image)
    {
        Id = id;
        Name = name;
        Price = price;
        Amount = amount;
        Image = image;
    }
}