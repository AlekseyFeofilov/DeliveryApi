using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.BasketService;

public class BasketService : IBasketService
{
    private readonly ApplicationDbContext _context;

    public BasketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DishBasketDto>?> GetCart(Guid userId)
    {
        var user = await _context.Users
            .Include(x => x.Cart)
            .ThenInclude(x => x.Dish)
            .SingleOrDefaultAsync(x => x.Id == userId);

        return user?.Cart
            .Select(basket =>
                new DishBasketDto(basket.Id, basket.Dish.Name, basket.Dish.Price, basket.Amount, basket.Dish.Image))
            .ToList();
    }

    public async Task<bool> AddBasket(Guid dishId, Guid userId)
    {
        var user = await _context.Users
            .Include(x => x.Cart)
            .ThenInclude(x => x.Dish)
            .SingleOrDefaultAsync(x => x.Id == userId);//todo split this to different services

        if (user == null) return false;
        
        var basket = user.Cart.SingleOrDefault(x => x.Dish.Id == dishId);

        if (basket != null)
        {
            basket.Amount++;
        }
        else
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(x => x.Id == dishId);
            if (dish == null) return false;
            _context.DishBaskets.Add(new DishBasket
            {
                Amount = 1,
                Dish = dish,
                Id = new Guid(),
                User = user
            });
        }
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBasket(Guid dishId, Guid userId, bool increase = false)
    {
        var basket = await _context.DishBaskets
            .SingleOrDefaultAsync(x => x.Dish.Id == dishId && x.User.Id == userId);

        if (basket == null) return false;

        if (!increase || basket.Amount == 1)
        {
            _context.DishBaskets.Remove(basket);
        }
        else
        {
            basket.Amount--;
        }
        
        await _context.SaveChangesAsync();
        return true;
    }
}