using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class OrderCreateDto
{
    [JsonPropertyName("deliveryTime")]
    [Required]
    //[DateRange(0)]
    public DateTime DeliveryTime { get; }

    [JsonPropertyName("address")]
    [Required]
    [MinLength(1)]
    public string Address { get; }

    public OrderCreateDto(DateTime deliveryTime, string address)
    {
        DeliveryTime = deliveryTime;
        Address = address;
    }
}