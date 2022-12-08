using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.Dto;

public class OrderDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("deliveryTime")]
    [Required]
    [DateRange(0)]
    public DateTime DeliveryTime { get; set; }

    [JsonPropertyName("orderTime")]
    [Required]
    [DateRange(laterThanTodayBy: 0)]
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
    public IEnumerable<DishBasketDto> DishBaskets { get; set; }

    [JsonPropertyName("address")]
    [Required]
    [MinLength(1)]
    public string Address { get; set; }
}