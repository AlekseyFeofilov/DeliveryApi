using DeliveryAppAPI.DbContexts;
using DeliveryAppAPI.Models.DbSets;
using DeliveryAppAPI.Models.Dto;
using DeliveryAppAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryAppAPI.Services.DishServices;

public class DishServices : IDishService
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 5;

    public DishServices(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DishDto>> GetAllDishes(DishCategory? categories, DishSorting? sorting, int? page,
        bool vegetarian = false)
    {
        var dishes = _context.Dishes.AsQueryable();

        if (vegetarian)
        {
            dishes = dishes.Where(x => x.Vegetarian);
        }

        if (categories.HasValue) 
        {
            dishes = dishes.Where(x => x.Category == categories);
        }

        dishes = Sort(sorting, dishes);

        return await dishes
            .Skip((page ?? 1 - 1) * PageSize)
            .Take(PageSize)
            .Select(x => new DishDto(
                x.Id,
                x.Name,
                x.Description,
                x.Price,
                x.Image,
                x.Vegetarian,
                x.Rating,
                x.Category
            ))
            .ToListAsync();
    }

    private IQueryable<Dish> Sort(DishSorting? sorting, IQueryable<Dish> dishes)
    {
        return sorting switch
        {
            DishSorting.NameAsc => dishes.OrderBy(x => x.Name),
            DishSorting.NameDesc => dishes.OrderByDescending(x => x.Name),
            DishSorting.PriceAsc => dishes.OrderBy(x => x.Price),
            DishSorting.PriceDesc => dishes.OrderByDescending(x => x.Price),
            DishSorting.RatingAsc => dishes.OrderBy(x => x.Rating),
            DishSorting.RatingDesc => dishes.OrderByDescending(x => x.Rating),
            null => dishes,
            _ => throw new ArgumentOutOfRangeException(nameof(sorting), sorting, null)
        };
    }
}