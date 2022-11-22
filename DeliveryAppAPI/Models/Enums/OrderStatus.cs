using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    InProcess, 
    Delivered
}