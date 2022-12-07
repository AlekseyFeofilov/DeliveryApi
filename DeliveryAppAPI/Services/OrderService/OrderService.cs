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

    public async Task<OrderDto> GetOrderDto(Order order)
    {
        return new OrderDto(
            order.Id,
            order.DeliveryTime,
            order.OrderTime,
            order.Status,
            order.Price,
            await GetOrderDishBaskets(order),
            order.Address
        );
    }

    public async Task<IEnumerable<OrderInfoDto>> GetAllOrders(Guid userId)
    {
        return await _context.Orders
            .Where(x => x.User.Id == userId)
            .Select(x => new OrderInfoDto(x.Id, x.DeliveryTime, x.OrderTime, x.Status, x.Price))
            .ToListAsync();
    }

    public async Task<bool> CreateOrder(OrderCreateDto orderCreateDto, User user)
    {
        var cart = await GetOrderDishBaskets(user.Id);
        if (!cart.Any()) return false;
        
        await _context.Orders.AddAsync(CreateOrder(orderCreateDto, cart, user));
        EmptyUserCart(user);
        
        await _context.SaveChangesAsync();
        return true;
    }

    public void ConfirmOrderDelivery(Order order)
    {
        order.Status = OrderStatus.Delivered;
        _context.SaveChangesAsync();
    }

    private async Task<ICollection<DishBasketDto>> GetOrderDishBaskets(Order order)
    {
        return await _context.DishBaskets
            .Where(x => order.DishBaskets.Contains(x))
            .Select(x => new DishBasketDto(x.Id, x.Dish.Name, x.Dish.Price, x.Amount, x.Dish.Image))
            .ToListAsync();
    }

    private async Task<ICollection<DishBasket>> GetOrderDishBaskets(Guid userId)
    {
        return await _context.DishBaskets
            .Where(x => x.User != null && x.User.Id == userId)
            .ToListAsync();
    }

    private Order CreateOrder(OrderCreateDto orderCreateDto, ICollection<DishBasket> cart, User user)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            DeliveryTime = orderCreateDto.DeliveryTime, //todo check the laboratory description
            OrderTime = DateTime.Now,
            Status = OrderStatus.InProcess,
            Price = cart.Sum(x => x.Amount * x.Dish.Price),
            DishBaskets = cart,
            Address = orderCreateDto.Address,
            User = user
        };
    }

    private void EmptyUserCart(User user)
    {
        foreach (var dishBasket in user.Cart)
        {
            dishBasket.User = null;
        }
    }
}