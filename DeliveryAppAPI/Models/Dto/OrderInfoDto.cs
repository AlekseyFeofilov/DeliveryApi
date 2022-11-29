using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class OrderInfoDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; }
    [JsonPropertyName("deliveryTime")]
    [Required]
    public DateTime DeliveryTime { get; }
    [JsonPropertyName("orderTime")]
    [Required]
    public DateTime OrderTime { get; }
    [JsonPropertyName("status")]
    [Required]
    public OrderStatus Status { get; }
    [JsonPropertyName("price")]
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; }

    public OrderInfoDto(Guid id, DateTime deliveryTime, DateTime orderTime, OrderStatus status, double price)
    {
        Id = id;
        DeliveryTime = deliveryTime;
        OrderTime = orderTime;
        Status = status;
        Price = price;
    }
}