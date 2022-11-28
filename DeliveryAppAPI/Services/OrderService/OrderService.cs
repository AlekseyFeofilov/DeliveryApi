using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<OrderDto?> GetOrder(Guid id)
    {
        var order = await _context.Orders
            .Include(x => x.DishBaskets)
            .ThenInclude(x => x.Dish)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (order == null) return null;

        var dishBaskets = order.DishBaskets
            .Select(x => new DishBasketDto(x.Id, x.Dish.Name, x.Dish.Price, x.Amount, x.Dish.Image))
            .ToList();

        return new OrderDto(order.Id, order.DeliveryTime, order.OrderTime, order.Status, order.Price, dishBaskets,
            order.Address);
    }

    public async Task<IEnumerable<OrderInfoDto>?> GetAllOrders(Guid userId)
    {
        var user = await _context.Users
            .Include(x => x.Orders)
            .SingleOrDefaultAsync(x => x.Id == userId);

        if (user == null) return null;

        var orders = user.Orders
            .Select(x => new OrderInfoDto(x.Id, x.DeliveryTime, x.OrderTime, x.Status, x.Price));
        
        return orders;
    }

    public async Task<bool> CreateOrder(OrderCreateDto orderCreateDto, Guid userId)
    {
        var user = await _context.Users
            .Include(x => x.Cart)
            .ThenInclude(x => x.Dish)
            .SingleOrDefaultAsync(x => x.Id == userId);
        
        if (user == null) return false;

        var cart =  user.Cart;

        if (cart.Count == 0)
        {
            return false;
        }
        
        await _context.Orders.AddAsync(new Order()
        {
            Id = Guid.NewGuid(),
            DeliveryTime = orderCreateDto.DeliveryTime,
            OrderTime = DateTime.Now,
            Status = OrderStatus.InProcess,
            Price = cart.Sum(x => x.Amount * x.Dish.Price),
            DishBaskets = cart,
            Address = orderCreateDto.Address,
            User = user
        });

        foreach (var dishBasket in user.Cart)
        {
            dishBasket.User = null;
        }
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ConfirmOrderDelivery(Guid id)
    {
        var order = await _context.Orders
            .SingleOrDefaultAsync(x => x.Id == id);

        if (order == null) return false;
        
        order.Status = OrderStatus.Delivered;
        await _context.SaveChangesAsync();
        return true;
    }
}