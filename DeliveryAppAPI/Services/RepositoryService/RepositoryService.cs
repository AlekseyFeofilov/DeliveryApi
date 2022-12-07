using System.Security.Claims;
using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Exceptions;
using DeliveryAppAPI.Models.DbSets;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.RepositoryService;

public class RepositoryService : IRepositoryService
{
    private readonly ApplicationDbContext _context;

    public RepositoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUser(ClaimsPrincipal claims)
    {
        var id = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
        if (user == null) throw new NotFoundException();
        
        return user;
    }
    
    public async Task<Dish> GetDish(Guid id)
    {
        var dish = await _context.Dishes.SingleOrDefaultAsync(x => x.Id == id);
        if (dish == null) throw new NotFoundException();
        
        return dish;
    }
    
    public async Task<Order> GetOrder(Guid id)
    {
        var order = await _context.Orders
            .Include(x => x.DishBaskets)
            .ThenInclude(x => x.Dish)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (order == null) throw new NotFoundException();
        return order;
    }

    public async Task<DishBasket?> GetDishBasket(Guid dishId, Guid userId)
    {
        return await _context.DishBaskets
            .SingleOrDefaultAsync(x =>
                x.User != null
                && x.Dish.Id == dishId
                && x.User.Id == userId
            );
    }
}