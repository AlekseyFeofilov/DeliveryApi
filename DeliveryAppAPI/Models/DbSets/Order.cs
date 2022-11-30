using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;
#pragma warning disable CS8618

namespace DeliveryAppAPI.Models.DbSets;

public class Order
{
    public Guid Id { get; set; }
    [DateRange(0)]
    public DateTime DeliveryTime { get; set; }
    [DateRange(laterThanTodayBy: 0)]
    public DateTime OrderTime { get; set; }
    public OrderStatus Status { get; set; }
    [Range(0, double.MaxValue)] public double Price { get; set; }
    [Required] public ICollection<DishBasket> DishBaskets { get; set; }
    public string Address { get; set; } //todo add migration
    public User User { get; set; }
}