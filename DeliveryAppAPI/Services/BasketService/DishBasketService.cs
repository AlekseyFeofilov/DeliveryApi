using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.BasketService;

public class DishBasketService : IDishBasketService
{
    private readonly ApplicationDbContext _context;

    public DishBasketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DishBasketDto>> GetCart(Guid userId)
    {
        return await _context.DishBaskets
            .Where(x => x.User != null && x.User.Id == userId)
            .Select(dishBasket =>
                new DishBasketDto(dishBasket.Id, dishBasket.Dish.Name, dishBasket.Dish.Price, dishBasket.Amount,
                    dishBasket.Dish.Image))
            .ToListAsync();
    }

    public async Task AddBasket(Dish dish, User user)
    {
        var dishBasket = await GetDishBasket(dish.Id);
        AddDishBasket(dishBasket, dish, user);
    }

    public void DeleteBasket(DishBasket dishBasket, bool increase = false)
    {
        if (!increase || dishBasket.Amount == 1)
        {
            _context.DishBaskets.Remove(dishBasket);
        }
        else
        {
            dishBasket.Amount--;
        }

        _context.SaveChangesAsync();
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

    private void AddDishBasket(DishBasket? dishBasket, Dish dish, User user)
    {
        if (dishBasket != null)
        {
            dishBasket.Amount++;
        }
        else
        {
            _context.DishBaskets.Add(new DishBasket
            {
                Amount = 1,
                Dish = dish,
                Id = new Guid(),
                User = user
            });
        }
        
        _context.SaveChangesAsync();
    }

    private async Task<DishBasket?> GetDishBasket(Guid dishId)
    {
        return await _context.DishBaskets
            .SingleOrDefaultAsync(x =>
                x.User != null
                && x.User.Id == x.Id
                && x.Dish.Id == dishId
            );
    }
}