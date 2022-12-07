using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class OrderDto
{
    [JsonPropertyName("id")] public Guid Id { get; }

    [JsonPropertyName("deliveryTime")]
    [Required]
    [DateRange(0)]
    public DateTime DeliveryTime { get; }

    [JsonPropertyName("orderTime")]
    [Required]
    [DateRange(laterThanTodayBy: 0)]
    public DateTime OrderTime { get; }

    [JsonPropertyName("status")]
    [Required]
    public OrderStatus Status { get; }

    [JsonPropertyName("price")]
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; }

    [JsonPropertyName("dishes")]
    [Required]
    public IEnumerable<DishBasketDto> DishBaskets { get; set; }

    [JsonPropertyName("address")]
    [Required]
    [MinLength(1)]
    public string Address { get; }

    public OrderDto(Guid id, DateTime deliveryTime, DateTime orderTime, OrderStatus status,
        double price /*, IEnumerable<DishBasketDto> dishBaskets*/, string address)
    {
        Id = id;
        DeliveryTime = deliveryTime;
        OrderTime = orderTime;
        Status = status;
        Price = price;
        //DishBaskets = dishBaskets;
        Address = address;
    }
}