using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class OrderInfoDto
{
    [JsonPropertyName("id")]
    public Guid Id;
    [JsonPropertyName("deliveryTime")]
    public DateTime DeliveryTime;
    [JsonPropertyName("orderTime")]
    public DateTime OrderTime;
    [JsonPropertyName("status")]
    public OrderStatus Status;
    [JsonPropertyName("price")]
    [Range(0, double.MaxValue)]
    public double Price;

    public OrderInfoDto(Guid id, DateTime deliveryTime, DateTime orderTime, OrderStatus status, double price)
    {
        Id = id;
        DeliveryTime = deliveryTime;
        OrderTime = orderTime;
        Status = status;
        Price = price;
    }
}