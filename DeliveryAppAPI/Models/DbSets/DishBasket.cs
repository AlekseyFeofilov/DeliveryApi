using System.ComponentModel.DataAnnotations;

namespace DeliveryAppAPI.Models.DbSets;

public class DishBasket
{
    public Guid Id { get; set; }
    [MinLength(1)]
    [Range(0, int.MaxValue)]
    public int Amount { get; set; }
    public string? Image { get; set; }
    public Order Order { get; set; }
    public Guid DishId { get; set; }
    public Dish Dish { get; set; }
}