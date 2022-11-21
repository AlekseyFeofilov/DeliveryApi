using System.ComponentModel.DataAnnotations;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;

namespace DeliveryAppAPI.Models.DbSets;

public class Order
{
    [Key]
    public Guid Id;
    public DateTime DeliveryTime;
    public DateTime OrderTime;
    public OrderStatus Status;
    [Range(0, double.MaxValue)]
    public double Price;
    [Required]
    public DishBasketDto[] Dishes;
    public string Address;
    public User User;

    public Order(Guid id, DateTime deliveryTime, DateTime orderTime, OrderStatus status, double price, DishBasketDto[] dishes, string address, User user)
    {
        Id = id;
        DeliveryTime = deliveryTime;
        OrderTime = orderTime;
        Status = status;
        Price = price;
        Dishes = dishes;
        Address = address;
        User = user;
    }
}