using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class OrderInfoDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("deliveryTime")]
    public DateTime DeliveryTime { get; set; }
    [JsonPropertyName("orderTime")]
    public DateTime OrderTime { get; set; }
    [JsonPropertyName("status")]
    public OrderStatus Status { get; set; }
    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    public OrderInfoDto(Guid id, DateTime deliveryTime, DateTime orderTime, OrderStatus status, double price)
    {
        Id = id;
        DeliveryTime = deliveryTime;
        OrderTime = orderTime;
        Status = status;
        Price = price;
    }
}