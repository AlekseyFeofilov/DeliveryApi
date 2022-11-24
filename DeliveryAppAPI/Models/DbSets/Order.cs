using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.DbSets;

public class Order
{
    public Guid Id { get; set; }
    public DateTime DeliveryTime { get; set; }
    public DateTime OrderTime { get; set; }
    public OrderStatus Status { get; set; }
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    [Required]
    public ICollection<DishBasket> DishBaskets { get; set; }
    public string Address { get; set; }
    public User User { get; set; }
}