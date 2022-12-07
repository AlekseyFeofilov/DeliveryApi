using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Attributes.ValidationAttributes;
using DeliveryAppAPI.Models.Enums;

#pragma warning disable CS8618

namespace DeliveryAppAPI.Models.DbSets;

public class Order
{
    public Guid Id { get; set; }
    [Required] [DateRange(0)] public DateTime DeliveryTime { get; set; }

    [Required]
    [DateRange(laterThanTodayBy: 0)]
    public DateTime OrderTime { get; set; }

    [Required] public OrderStatus Status { get; set; }
    [Required] [Range(0, double.MaxValue)] public double Price { get; set; }
    [Required] public ICollection<DishBasket> DishBaskets { get; set; }
    [MinLength(1)] [Required] public string Address { get; set; }
    public User User { get; set; }
}