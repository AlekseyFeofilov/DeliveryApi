using System.ComponentModel.DataAnnotations;

namespace DeliveryAppAPI.Models.DbSets;

public class DishBasket
{
    public Guid Id { get; set; }
    [Range(0, int.MaxValue)]
    public int Amount { get; set; }
    public User? User { get; set; }
    public Dish Dish { get; set; }
}