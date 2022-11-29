using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class OrderDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("deliveryTime")]
    [Required]
    public DateTime DeliveryTime { get; set; }
    [JsonPropertyName("orderTime")]
    [Required]
    public DateTime OrderTime { get; set; }
    [JsonPropertyName("status")]
    [Required]
    public OrderStatus Status { get; set; }
    [JsonPropertyName("price")]
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    [JsonPropertyName("dishes")]
    [Required]
    public IEnumerable<DishBasketDto> Dishes { get; set; }
    [JsonPropertyName("address")]
    [Required]
    [MinLength(1)]
    public string Address { get; set; }

    public OrderDto(Guid id, DateTime deliveryTime, DateTime orderTime, OrderStatus status, double price, IEnumerable<DishBasketDto> dishes, string address)
    {
        Id = id;
        DeliveryTime = deliveryTime;
        OrderTime = orderTime;
        Status = status;
        Price = price;
        Dishes = dishes;
        Address = address;
    }
}