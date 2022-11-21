using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Dto;

public class OrderCreateDto
{
    [JsonPropertyName("deliveryTime")]
    public DateTime DeliveryTime;
    [JsonPropertyName("address")]
    [MinLength(1)]
    public string Address;

    public OrderCreateDto(DateTime deliveryTime, string address)
    {
        DeliveryTime = deliveryTime;
        Address = address;
    }
}