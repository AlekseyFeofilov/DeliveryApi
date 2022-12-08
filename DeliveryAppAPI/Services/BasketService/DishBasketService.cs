using AutoMapper;
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
    private readonly IMapper _mapper;

    public DishBasketService(ApplicationDbContext context, IRepositoryService repositoryService, IMapper mapper)
    {
        _context = context;
        _repositoryService = repositoryService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DishBasketDto>> GetCart(Guid userId)
    {
        var dishBaskets = await _context.DishBaskets
            .Where(x => x.User != null && x.User.Id == userId)
            .Include(x => x.Dish)
            .ThenInclude(x => x.Reviews)
            .ToListAsync();

        return dishBaskets.Select(x => _mapper.Map<DishBasketDto>(x));
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