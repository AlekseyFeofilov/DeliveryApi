using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class OrderCreateDto
{
    [JsonPropertyName("deliveryTime")]
    [Required]
    public DateTime DeliveryTime { get; set; }
    [JsonPropertyName("address")]
    [Required]
    [MinLength(1)]
    public string Address { get; set; }

    public OrderCreateDto(DateTime deliveryTime, string address)
    {
        DeliveryTime = deliveryTime;
        Address = address;
    }
}