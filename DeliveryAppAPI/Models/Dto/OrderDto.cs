using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class OrderDto
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
    [JsonPropertyName("dishes")]
    public DishBasketDto[] Dishes;
    [JsonPropertyName("address")]
    public string Address;

    public OrderDto(Guid id, DateTime deliveryTime, DateTime orderTime, OrderStatus status, double price, DishBasketDto[] dishes, string address)
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