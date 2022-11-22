using System.Text.Json.Serialization;

namespace DeliveryAppAPI.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok, 
    Pizza, 
    Soup, 
    Dessert, 
    Drink
}