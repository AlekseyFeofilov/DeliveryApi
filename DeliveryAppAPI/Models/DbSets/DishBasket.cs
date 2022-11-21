using System.ComponentModel.DataAnnotations;

namespace DeliveryAppAPI.Models.DbSets;

public class DishBasket
{
    [Key]
    public Guid Id;
    [MinLength(1)]
    public string Name;
    [Range(0, double.MaxValue)]
    public double Price;
    [Range(0, int.MaxValue)]
    public int Amount;
    public string? Image;
    public Order Order;

    public DishBasket(Guid id, string name, double price, int amount, string? image, Order order)
    {
        Id = id;
        Name = name;
        Price = price;
        Amount = amount;
        Image = image;
        Order = order;
    }
}