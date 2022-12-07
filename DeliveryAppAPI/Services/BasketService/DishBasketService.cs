using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Services.RepositoryService;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAppAPI.Services.BasketService;

public class DishBasketService : IDishBasketService
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryService _repositoryService;

    public DishBasketService(ApplicationDbContext context, IRepositoryService repositoryService) //todo квартс библиотека для фоновых процессов
    {
        _context = context;
        _repositoryService = repositoryService;
    }

    public async Task<IEnumerable<DishBasketDto>> GetCart(Guid userId)
    {
        return await _context.DishBaskets
            .Where(x => x.User != null && x.User.Id == userId)
            .Select(dishBasket => new DishBasketDto(
                dishBasket.Id,
                dishBasket.Dish.Name,
                dishBasket.Dish.Price,
                dishBasket.Amount, //todo
                dishBasket.Dish.Image
            ))
            .ToListAsync();
    }

    public async Task AddBasket(Dish dish, User user)
    {
        var dishBasket = await _repositoryService.GetDishBasket(dish.Id, user.Id);
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
}